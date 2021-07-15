using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Messages
{
    public class VendorOrderRequest : IVendorOrderRequest
    {
        public string CatalogId { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
    }
}
