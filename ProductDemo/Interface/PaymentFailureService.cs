
using Microsoft.IdentityModel.Tokens;
using ProductDemo.Models;

namespace ProductDemo.Interface
{
    public class PaymentFailureService : IPaymentFailureService
    {

        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        public PaymentFailureService(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        public async Task HandlePaymentFailureAsync(PaymentFailure paymentFailure)
        {
            try
            {
                var transactionId = paymentFailure.TransactionId;
                var amount = paymentFailure.Amount;
                var productInfo = paymentFailure.ProductInfo;
                var firstName = paymentFailure.FirstName;
                var email = paymentFailure.Email;
                var phone = paymentFailure.Phone;

                if (string.IsNullOrEmpty(transactionId) || string.IsNullOrEmpty(amount))
                {
                    _logger.LogWarning("HandlePaymentFailureAsync: Missing required form fields.");
                    throw new ArgumentException("Missing required form fields.");
                }

                var payment = new Payments
                {
                    TransactionId = transactionId,
                    Amount = decimal.Parse(amount),
                    Currency = "INR",
                    Status = "Failed",
                    PaymentDate = DateTime.UtcNow,
                    ProductInfo = productInfo,
                    FirstName = firstName,
                    Email = email,
                    Phone = phone
                };

                await _paymentService.SavePaymentAsync(payment);
                _logger.LogInformation("Payment failure recorded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in HandlePaymentFailureAsync.");
                throw;
            }
            
        }
    }
}

