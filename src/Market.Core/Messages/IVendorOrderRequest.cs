namespace Market.Core.Messages
{
    public interface IVendorOrderRequest
    {
        decimal Amount { get; set; }
        string CatalogId { get; set; }
        string Vendor { get; set; }
    }
}