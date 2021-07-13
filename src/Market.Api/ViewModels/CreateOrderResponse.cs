using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Api.ViewModels
{
    public class CreateOrderResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
