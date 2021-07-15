using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.DTO
{
    public class Catalog : ICatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public decimal Amount { get; set; }
        public string ExtraFieldOne { get; set; }
        public string ExtraFieldTwo { get; set; }
        public virtual IVendor Vendor { get; set; }
    }
}
