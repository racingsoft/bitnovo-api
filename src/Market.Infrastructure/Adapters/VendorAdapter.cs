using Market.Core.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Market.Core.DTO;
using Messages = Market.Core.Messages;

namespace Market.Infrastructure.Adapters
{
    public abstract class VendorAdapter : IVendorAdapter
    {
        public virtual DTO.IVendor Vendor { get => throw new NotImplementedException(); }

        public virtual IList<DTO.ICatalog> GetCatalog()
        {
            throw new NotImplementedException();
        }

        public virtual Messages.IVendorOrderResponse CreateOrder(
            Messages.IVendorOrderRequest vendorOrderRequest)
        {
            throw new NotImplementedException();
        }
    }
}
