using Market.Core.Factories;
using DTO = Market.Core.DTO;
using System;
using System.Collections.Generic;
using Market.Core.Messages;
using Market.Infrastructure.Adapters;

namespace Market.Api.Vendor
{
    public class VendorTwoCatalog
    {
        public string Name { get; set; }
        public string Ean { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }

    public class VendorTwoCatalogConverter
    {
        private readonly IFactory<DTO.ICatalog> _catalogFactory;
        private readonly IFactory<DTO.IVendor> _vendorFactory;

        public VendorTwoCatalogConverter(
            IFactory<DTO.ICatalog> catalogFactory,
            IFactory<DTO.IVendor> vendorFactory)
        {
            _catalogFactory = catalogFactory;
            _vendorFactory = vendorFactory;
        }

        public DTO.ICatalog ConvertToDTO(VendorTwoCatalog catalog)
        {
            
            var vendorDTO = _vendorFactory.Create();
            vendorDTO.Id = 2;
            vendorDTO.Name = "vendorTwo";

            var catalogDTO = _catalogFactory.Create();
            catalogDTO.Name = catalog.Name;
            catalogDTO.ExternalId = catalog.Ean;
            catalogDTO.ExtraFieldOne = catalog.Vendor;
            catalogDTO.Amount = catalog.Amount;
            catalogDTO.Vendor = vendorDTO;

            return catalogDTO;
        }
    }

    public class VendorTwoResponse
    {
        public string TxtId { get; set; }
        public string Ean { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorTwoResponseConverter
    {
        private readonly IFactory<IVendorOrderResponse> _vendorOrderResponseFactory;

        public VendorTwoResponseConverter(IFactory<IVendorOrderResponse> vendorOrderResponseFactory)
        {
            _vendorOrderResponseFactory = vendorOrderResponseFactory;
        }

        public IVendorOrderResponse ConvertToDTO(VendorTwoResponse response)
        {
            var responseDTO = _vendorOrderResponseFactory.Create();

            responseDTO.OrderId = response.TxtId;
            responseDTO.ProductId = response.Ean;
            responseDTO.Amount = response.Amount;

            return responseDTO;
        }
    }

    public class VendorTwoRequest
    {
        public string Ean { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorTwoRequestConverter
    {
        public VendorTwoRequest ConvertToVendorTwoRequest(IVendorOrderRequest request)
        {
            return new VendorTwoRequest()
            {
                Ean = request.CatalogId,
                Type = request.Vendor,
                Amount = request.Amount,
            };
        }
    }

    public class VendorTwo : VendorAdapter
    {
        private readonly VendorTwoCatalogConverter _catalogConverter;
        private readonly VendorTwoRequestConverter _requestConverter;
        private readonly VendorTwoResponseConverter _responseConverter;

        private readonly DTO.IVendor _vendor;

        public VendorTwo(
            VendorTwoCatalogConverter catalogConverter,
            VendorTwoRequestConverter requestConverter,
            VendorTwoResponseConverter responseConverter,
            IFactory<DTO.IVendor> vendorFactory)
        {
            _catalogConverter = catalogConverter;
            _requestConverter = requestConverter;
            _responseConverter = responseConverter;

            _vendor = vendorFactory.Create();
            _vendor.Id = 2;
            _vendor.Name = "vendorTwo";
        }

        public override DTO.IVendor Vendor => _vendor;

        public override IList<DTO.ICatalog> GetCatalog()
        {
            #region [DO NOT TOUCH THIS]

            Random rnd = new Random();
            var catalog = new List<VendorTwoCatalog>();
            for (int i = 3; i <= 7; i++)
            {
                catalog.Add(new VendorTwoCatalog
                {
                    Amount = rnd.Next(50, 100),
                    Type = "Sale",
                    Name = $"Product {i}",
                    Ean = i.ToString(),
                    Vendor = $"Verdor{i}"
                });
            }

            #endregion [DO NOT TOUCH THIS]

            return ConvertToDTO(catalog);
        }

        public override IVendorOrderResponse CreateOrder(IVendorOrderRequest vendorOrderRequest)
        {
            var request = _requestConverter.ConvertToVendorTwoRequest(vendorOrderRequest);

            #region [DO NOT TOUCH THIS]

            if (string.IsNullOrWhiteSpace(request.Ean))
            {
                throw new Exception("Txid is null");
            }
            if (string.IsNullOrWhiteSpace(request.Type))
            {
                throw new Exception("Type is null");
            }
            //TODO: return new VendorTwoResponse
            var response = new VendorTwoResponse { Ean = request.Ean, Amount = request.Amount, TxtId = Guid.NewGuid().ToString() };

            #endregion [DO NOT TOUCH THIS]

            return _responseConverter.ConvertToDTO(response);
        }

        private IList<DTO.ICatalog> ConvertToDTO(List<VendorTwoCatalog> catalog)
        {
            var result = new List<DTO.ICatalog>();
            catalog.ForEach(c => result.Add(_catalogConverter.ConvertToDTO(c)));
            return result;
        }
    }
}