namespace ProductDemo.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ProductInfo { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
