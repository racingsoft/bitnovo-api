using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Messages
{
    public class VendorOrderResponse : IVendorOrderResponse
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public decimal Amount { get; set; }
    }
}
