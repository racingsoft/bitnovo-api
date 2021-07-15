namespace Market.Core.DTO
{
    public interface IOrder
    {
        int Id { get; set; }
        decimal Amount { get; set; }
        int CatalogId { get; set; }
        string ExternalId { get; set; }
    }
}