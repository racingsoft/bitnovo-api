using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Messages
{
    public class CreateOrderResponse : ICreateOrderResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
