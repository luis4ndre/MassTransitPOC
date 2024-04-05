namespace PurchaseApi.DTO
{
    public class OrderDTO
    {
        public string? Client { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }
}
