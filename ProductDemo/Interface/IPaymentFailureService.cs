using ProductDemo.Models;

namespace ProductDemo.Interface
{
    public interface IPaymentFailureService
    {
        Task HandlePaymentFailureAsync(PaymentFailure paymentFailure);

    }
}
