namespace OrderProcessor
{
    public class Customer
    {
        public bool IsPremium { get; set; }
        public decimal AccountBalance { get; set; }
    }

    public class Order
    {
        public decimal Amount { get; set; }
        public bool IsRush { get; set; }
    }

    public class OrderResult
    {
        public bool IsApproved { get; set; }
        public string Message { get; set; }
    }
}