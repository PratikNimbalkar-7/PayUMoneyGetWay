using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductDemo.Models;

namespace ProductDemo.Interface
{
    public class PaymentService : IPaymentService
    {
        private readonly ProductDB productdb;

        public PaymentService(ProductDB productDB ) 
        { 
           this.productdb = productDB;
            
        }


        public Task SavePaymentAsync(Payments payments)
        {
            productdb.Payments.Add(payments);
            productdb.SaveChanges();
           return Task.CompletedTask;
        }
    }
}
