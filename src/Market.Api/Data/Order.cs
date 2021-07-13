namespace Market.Api.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public string ExternalId { get; set; }
        public decimal Amount { get; set; }
        public virtual Catalog Catalog { get; set; }
    }
}