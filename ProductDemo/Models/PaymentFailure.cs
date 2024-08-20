namespace ProductDemo.Models
{
    public class PaymentFailure
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string ProductInfo { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime FailureDate { get; set; }
    }
}
