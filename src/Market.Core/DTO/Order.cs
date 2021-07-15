using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.DTO
{
    public class Order : IOrder
    {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public string ExternalId { get; set; }
        public decimal Amount { get; set; }
    }
}
