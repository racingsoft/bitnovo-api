using System.Collections.Generic;

namespace Market.Api.Data
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public decimal Amount { get; set; }
        public string ExtraFieldOne { get; set; }
        public string ExtraFieldTwo { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}