namespace Market.Core.Messages
{
    public interface IVendorOrderResponse
    {
        decimal Amount { get; set; }
        string OrderId { get; set; }
        string ProductId { get; set; }
    }
}