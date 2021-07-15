using Market.Core.DTO;
using Market.Core.Messages;
using System.Collections.Generic;

namespace Market.Core.Adapters
{
    public interface IVendorAdapter
    {
        IVendor Vendor { get; }
        IVendorOrderResponse CreateOrder(IVendorOrderRequest vendorOrderRequest);
        IList<ICatalog> GetCatalog();
    }
}