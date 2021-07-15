namespace Market.Core.Messages
{
    public interface ICreateOrderResponse
    {
        decimal Amount { get; set; }
        int Id { get; set; }
    }
}