using Market.Core.Factories;
using Market.Core.Messages;
using Market.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Converters
{
    public class VendorOrderRequestConverter : IVendorOrderRequestConverter
    {
        private IFactory<IVendorOrderRequest> _vendorOrderRequestFactory;

        public VendorOrderRequestConverter(IFactory<IVendorOrderRequest> vendorOrderRequestFactory)
        {
            _vendorOrderRequestFactory = vendorOrderRequestFactory;
        }

        public IVendorOrderRequest ConvertToVendorOrderRequestMessage(Catalog producto)
        {
            var vendorOrderRequest = _vendorOrderRequestFactory.Create();

            vendorOrderRequest.Vendor = producto.ExtraFieldOne;
            vendorOrderRequest.CatalogId = producto.ExternalId;
            vendorOrderRequest.Amount = producto.Amount;

            return vendorOrderRequest;
        }
    }
}
