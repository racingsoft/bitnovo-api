using System.Collections.Generic;
using DTO = Market.Core.DTO;

namespace Market.Infrastructure.Entities
{
    public class Catalog : Entity
    {
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