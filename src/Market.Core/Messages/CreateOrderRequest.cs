using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Messages
{
    public class CreateOrderRequest : ICreateOrderRequest
    {
        public int Id { get; set; }
    }
}
