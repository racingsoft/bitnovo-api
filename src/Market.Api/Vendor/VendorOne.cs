using Market.Core.Factories;
using Market.Core.Messages;
using Market.Infrastructure.Adapters;
using System;
using System.Collections.Generic;
using DTO = Market.Core.DTO;
using Messages = Market.Core.Messages;

namespace Market.Api.Vendor
{
    public class VendorOneCatalog
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class VendorOneCatalogConverter
    {
        private readonly IFactory<DTO.ICatalog> _catalogFactory;
        private readonly IFactory<DTO.IVendor> _vendorFactory;

        public VendorOneCatalogConverter(
            IFactory<DTO.ICatalog> catalogFactory,
            IFactory<DTO.IVendor> vendorFactory)
        {
            _catalogFactory = catalogFactory;
            _vendorFactory = vendorFactory;
        }

        public DTO.ICatalog ConvertToDTO(VendorOneCatalog catalog)
        {
            var catalogDTO = _catalogFactory.Create();
            var vendorDTO = _vendorFactory.Create();

            vendorDTO.Id = 1;
            vendorDTO.Name = "vendorOne";

            catalogDTO.Name = catalog.Name;
            catalogDTO.ExternalId = catalog.Sku;
            catalogDTO.ExtraFieldOne = catalog.Vendor;
            catalogDTO.Amount = catalog.Amount;
            catalogDTO.Vendor = vendorDTO;

            return catalogDTO;
        }
    }

    public class VendorOneResponse
    {
        public string OrderIdentifier { get; set; }
        public string Sku { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorOneResponseConverter
    {
        private readonly IFactory<IVendorOrderResponse> _vendorOrderResponseFactory;

        public VendorOneResponseConverter(IFactory<IVendorOrderResponse> vendorOrderResponseFactory)
        {
            _vendorOrderResponseFactory = vendorOrderResponseFactory;
        }

        public IVendorOrderResponse ConvertToDTO(VendorOneResponse response)
        {
            var responseDTO = _vendorOrderResponseFactory.Create();

            responseDTO.OrderId = response.OrderIdentifier;
            responseDTO.ProductId = response.Sku;
            responseDTO.Amount = response.Amount;

            return responseDTO;
        }
    }

    public class VendorOneRequest
    {
        public string Sku { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorOneRequestConverter
    {
        public VendorOneRequest ConvertToVendorOneRequest(IVendorOrderRequest request)
        {
            return new VendorOneRequest()
            {
                Sku = request.CatalogId,
                Vendor = request.Vendor,
                Amount = request.Amount,
            };
        }
    }

    public class VendorOne : VendorAdapter
    {
        private readonly VendorOneCatalogConverter _catalogConverter;
        private readonly VendorOneRequestConverter _requestConverter;
        private readonly VendorOneResponseConverter _responseConverter;

        private readonly DTO.IVendor _vendor;

        public VendorOne(
            VendorOneCatalogConverter catalogConverter,
            VendorOneRequestConverter requestConverter,
            VendorOneResponseConverter responseConverter,
            IFactory<DTO.IVendor> vendorFactory)
        {
            _catalogConverter = catalogConverter;
            _requestConverter = requestConverter;
            _responseConverter = responseConverter;
            
            _vendor = vendorFactory.Create();
            _vendor.Id = 1;
            _vendor.Name = "vendorOne";
        }

        public override DTO.IVendor Vendor => _vendor;

        public override IList<DTO.ICatalog> GetCatalog()
        {
            #region [DO NOT TOUCH THIS]

            Random rnd = new Random();
            var catalog = new List<VendorOneCatalog>();
            for (int i = 1; i <= 5; i++)
            {
                catalog.Add(new VendorOneCatalog
                {
                    Amount = rnd.Next(50, 100),
                    CurrencyCode = "EUR",
                    Name = $"Product {i}",
                    Sku = i.ToString(),
                    Vendor = "Verdor 1"
                });
            }

            #endregion [DO NOT TOUCH THIS]

            return ConvertToDTO(catalog);
        }

        public override IVendorOrderResponse CreateOrder(IVendorOrderRequest vendorOrderRequest)
        {
            var request = _requestConverter.ConvertToVendorOneRequest(vendorOrderRequest);

            #region [DO NOT TOUCH THIS]

            if (string.IsNullOrWhiteSpace(request.Sku))
            {
                throw new Exception("Sku is null");
            }
            if (string.IsNullOrWhiteSpace(request.Vendor))
            {
                throw new Exception("Vendor is null");
            }

            //TODO: return new VendorOneResponse
            var response = new VendorOneResponse { Sku = request.Sku, Amount = request.Amount, OrderIdentifier = Guid.NewGuid().ToString() };

            #endregion [DO NOT TOUCH THIS]

            return _responseConverter.ConvertToDTO(response);
        }

        private IList<DTO.ICatalog> ConvertToDTO(List<VendorOneCatalog> catalog)
        {
            var result = new List<DTO.ICatalog>();
            catalog.ForEach(c => result.Add(_catalogConverter.ConvertToDTO(c)));
            return result;
        }
    }
}