using Market.Core.Adapters;
using Market.Core.Factories;
using Market.Core.Services;
using Market.Infrastructure.Converters;
using Market.Infrastructure.Entities;
using Market.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Market.Core.DTO;
using Messages = Market.Core.Messages;

namespace Market.Infrastructure.Services
{
    public class MarketService : IMarketService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly ICatalogConverter _catalogConverter;

        private readonly IVendorRepository _vendorRepository;

        private readonly IOrderRepository _orderRepository;
        private readonly IOrderConverter _orderConverter;

        private readonly IVendorOrderRequestConverter _vendorOrderRequestConverter;
        private readonly IFactory<DTO.IOrder> _orderDTOFactory;

        private readonly IVendorConfiguration _vendorConfiguration;

        private IEnumerable<IVendorAdapter> Vendors { get => _vendorConfiguration.Vendors.AsEnumerable(); }

        private IVendorAdapter GetVendorAdapter(int id)
        {
            return Vendors.SingleOrDefault(x => x.Vendor.Id == id);
        }

        public MarketService(
            IVendorConfiguration vendorConfiguration,
            ICatalogRepository catalogRepository, 
            ICatalogConverter catalogConverter,
            IVendorRepository vendorRepository,
            IOrderRepository orderRepository,
            IOrderConverter orderConverter,
            IVendorOrderRequestConverter vendorOrderRequestConverter,
            IFactory<DTO.IOrder> orderDTOFactory)
        {
            _vendorConfiguration = vendorConfiguration;
            _catalogRepository = catalogRepository;
            _catalogConverter = catalogConverter;

            _vendorRepository = vendorRepository;

            _orderRepository = orderRepository;
            _orderConverter = orderConverter;

            _vendorOrderRequestConverter = vendorOrderRequestConverter;
            _orderDTOFactory = orderDTOFactory;
        }

        public async Task<IList<DTO.ICatalog>> GetProductsAsync()
        {
            var result = new List<DTO.ICatalog>();

            await foreach (var catalog in _catalogRepository.ListAsync())
            {
                result.Add( _catalogConverter.ConvertToDTO(catalog));
            }

            return result;
        }

        public async Task SyncCatalogAsync()
        {
            var updatedCatalog = DownloadAllCatalogs();
            await UpdateCatalog(updatedCatalog);
        }

        private IList<DTO.ICatalog> DownloadAllCatalogs()
        {
            var updatedCatalog = new List<DTO.ICatalog>();

            foreach(var vendor in Vendors)
            {
                updatedCatalog.Merge(vendor.GetCatalog());
            }

            return updatedCatalog;
        }

        private async Task UpdateCatalog(IList<DTO.ICatalog> updatedCatalog)
        {
            foreach(var catalog in updatedCatalog)
            {
                await InsertOrUpdateAsync(catalog);
            }
        }

        private async Task InsertOrUpdateAsync(DTO.ICatalog catalog)
        {
            var catalogRepo = _catalogRepository
                .List(x => x.Name == catalog.Name)
                .SingleOrDefault();

            if(catalogRepo == null)
            {
                await InsertAsync(catalog);
            }
            else
            {
                await UpdateAsync(catalogRepo, catalog);
            }
        }

        private async Task InsertAsync(DTO.ICatalog catalog)
        {
            var catalogRepo = new Catalog();
            catalogRepo.Amount = catalog.Amount;
            catalogRepo.ExternalId = catalog.ExternalId;
            catalogRepo.ExtraFieldOne = catalog.ExtraFieldOne;
            catalogRepo.ExtraFieldTwo = catalog.ExtraFieldTwo;
            catalogRepo.Name = catalog.Name;
            catalogRepo.VendorId = catalog.Vendor.Id;

            await _catalogRepository.InsertAsync(catalogRepo);

            //var vendor = await _vendorRepository.GetByIdAsync(catalogRepo.VendorId);
            //vendor.Catalogs.Add(catalogRepo);
            //await _vendorRepository.UpdateAsync(vendor);
        }

        private async Task UpdateAsync(Catalog catalogRepo, DTO.ICatalog catalog)
        {
            //if (catalogRepo.VendorId != catalog.Vendor.Id)
            //{
            //    var vendor = await _vendorRepository.GetByIdAsync(catalogRepo.VendorId);
            //    vendor.Catalogs.Remove(catalogRepo);
            //    await _vendorRepository.UpdateAsync(vendor);
            //}

            catalogRepo.VendorId = catalog.Vendor.Id;
            catalogRepo.Amount = catalog.Amount;
            catalogRepo.ExternalId = catalog.ExternalId;
            catalogRepo.ExtraFieldOne = catalog.ExtraFieldOne;
            catalogRepo.ExtraFieldTwo = catalog.ExtraFieldTwo;
            catalogRepo.Name = catalog.Name;
            catalogRepo.VendorId = catalog.Vendor.Id;
            await _catalogRepository.UpdateAsync(catalogRepo);

            //var vendor2 = await _vendorRepository.GetByIdAsync(catalogRepo.VendorId);
            //vendor2.Catalogs.Add(catalogRepo);
            //await _vendorRepository.UpdateAsync(vendor2);
        }

        public async Task<DTO.IOrder> CreateOrderAsync(int id)
        {
            var producto = await GetProductByIdAsync(id);
            var vendorAdapter = GetVendorAdapter(producto.VendorId);

            var vendorOrderResponse = vendorAdapter.CreateOrder(
                _vendorOrderRequestConverter.ConvertToVendorOrderRequestMessage(producto));

            // TODO: VendorOrderException en VendorAdapter
            if (vendorOrderResponse == null)
            {
                throw new Exception("Error to processing payment");
            }

            Order order = await CreateOrder(id, producto, vendorOrderResponse);

            return _orderConverter.ConvertToDTO(order);
        }

        private async Task<Catalog> GetProductByIdAsync(int id)
        {
            var catalog = await _catalogRepository.GetByIdAsync(id);

            if(catalog == null)
            {
                throw new Exception("Product not found");
            }

            return catalog;
        }

        private async Task<Order> CreateOrder(int id, Catalog producto, Messages.IVendorOrderResponse vendorOrderResponse)
        {
            var order = new Order()
            {
                CatalogId = id,
                ExternalId = vendorOrderResponse.OrderId,
                Amount = producto.Amount
            };

            await _orderRepository.InsertAsync(order);
            return order;
        }
    }

    static class ListExtension
    {
        public static IList<DTO.ICatalog> Merge(this IList<DTO.ICatalog> destination, IList<DTO.ICatalog> source)
        {
            foreach (var catalog in source)
            {
                destination.Add(catalog.Minimum(destination.PopByCatalogName(catalog)));
            }

            return destination;
        }

        public static DTO.ICatalog Minimum(this DTO.ICatalog catalog, DTO.ICatalog other)
        {
            return (other != null && other.Amount < catalog.Amount) ? other : catalog;
        }

        public static DTO.ICatalog PopByCatalogName(this IList<DTO.ICatalog> source, DTO.ICatalog catalog)
        {
            var found = source.SingleOrDefault(x => x.Name == catalog.Name);
            if (found != null)
            {
                source.Remove(found);
            }
            return found;
        }
    }
}
