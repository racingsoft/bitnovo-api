using System.Collections.Generic;
using DTO = Market.Core.DTO;

namespace Market.Infrastructure.Entities
{
    public class Vendor : Entity
    {
        public string Name { get; set; }
        public ICollection<Catalog> Catalogs { get; set; }
    }
}