using ProductDemo.Models;

namespace ProductDemo.Interface
{
    public interface IPaymentService
    {
        Task SavePaymentAsync(Payments payments);

     
    }
}
