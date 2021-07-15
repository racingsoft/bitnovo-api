namespace Market.Infrastructure.Entities
{
    public class Order : Entity
    {
        public int CatalogId { get; set; }
        public string ExternalId { get; set; }
        public decimal Amount { get; set; }
        public virtual Catalog Catalog { get; set; }
    }
}