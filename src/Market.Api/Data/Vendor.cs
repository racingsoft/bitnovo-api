using System.Collections.Generic;

namespace Market.Api.Data
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Catalog> Catalogs { get; set; }
    }
}