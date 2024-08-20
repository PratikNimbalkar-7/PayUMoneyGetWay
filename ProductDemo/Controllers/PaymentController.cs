using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductDemo.Interface;
using ProductDemo.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDemo.Controllers
{
    public class PaymentController : Controller
    {
        private const string MerchantKey = "srPlMt";
        private const string MerchantSalt = "kUG7FpuDkRzDcPkZPJmBlcAa6DnceZBu";
        private const string PayUBaseURL = "https://test.payu.in/_payment";

        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentFailureService _paymentFailureService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ICartService cartService, IPaymentService paymentService, IConfiguration configuration,ILogger<PaymentController> logger, IPaymentFailureService paymentFailureService)
        {
            _cartService = cartService;
            _paymentService = paymentService;
            _configuration = configuration;
            _logger = logger;
            _paymentFailureService = paymentFailureService;
        }

        [HttpPost]
        public IActionResult ProcessPayment(string firstName, string email, string phone, string address, Products products)
        {
            var cartItems = _cartService.GetCartItems();
            var totalAmount = cartItems.Sum(item => item.Pro_Prize * item.Quantity);
            var transactionId = Guid.NewGuid().ToString();
            var productInfo = cartItems.FirstOrDefault()?.Pro_Name;

            string hashString = $"{MerchantKey}|{transactionId}|{totalAmount}|{productInfo}|{firstName}|{email}|||||||||||{MerchantSalt}";
            string hash = PayUHelper.GenerateHash(hashString);

            ViewBag.MerchantKey = MerchantKey;
            ViewBag.TransactionId = transactionId;
            ViewBag.Amount = totalAmount;
            ViewBag.ProductInfo = productInfo;
            ViewBag.FirstName = firstName;
            ViewBag.Email = email;
            ViewBag.Phone = phone;
            ViewBag.Address = address;
            ViewBag.Surl = Url.Action("PaymentSuccess", "Payment", null, protocol: Request.Scheme);
            ViewBag.Furl = Url.Action("PaymentFailure", "Payment", null, protocol: Request.Scheme);
            ViewBag.Hash = hash;
            ViewBag.ServiceProvider = "payu_paisa";
            ViewBag.PayUBaseURL = PayUBaseURL;

            return View("ProcessPayment");
        }

        public async Task<IActionResult> PaymentSuccess()
        {
            try
            {
                var payment = new Payments
                {
                    TransactionId = Request.Form["txnid"], 
                    Amount = decimal.Parse(Request.Form["amount"]),
                    Currency = "INR", // Assuming INR as currency
                    Status = "Success",
                    PaymentDate = DateTime.UtcNow,
                    ProductInfo = Request.Form["productinfo"],
                    FirstName = Request.Form["firstname"],
                    Email = Request.Form["email"],
                    Phone = Request.Form["phone"]
                };

                await _paymentService.SavePaymentAsync(payment);
            }
            catch (Exception ex)
            {
                // Log the exception details (using your preferred logging framework)
                Console.WriteLine($"PaymentSuccess Error: {ex.Message}");
                return View("Error");
            }

            return View();
        }


        //public async Task<IActionResult> PaymentFailure()
        //{
        //    try
        //    {
        //        var paymentFailure = new PaymentFailure
        //        {
        //            TransactionId = paymentFailure.TransactionId
        //            Amount = ,
        //            ProductInfo = Request.Form["productinfo"],
        //            FirstName = Request.Form["firstname"],
        //            Email = Request.Form["email"],
        //            Phone = Request.Form["phone"],
        //            FailureMessage = Request.Form["error_Message"],
        //            FailureCode = Request.Form["error_code"],
        //            FailureDate = DateTime.UtcNow
        //        } 

        //        await _paymentFailureService.SavePaymentFailureAsync(paymentFailure);

        //        _logger.LogWarning("Payment failure recorded for Transaction ID: {TransactionId}", paymentFailure.TransactionId);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception details
        //        _logger.LogError(ex, "Error occurred in PaymentFailure.");
        //        return View("Error");
        //    }

        //    return View();
        //}


        //[HttpPost]
        //public async Task<IActionResult> RefundPayment(string transactionId, string reason)
        //{
        //    if (string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(reason))
        //    {
        //        return BadRequest("Transaction ID and reason are required.");
        //    }

        //    // Fetch the payment details from the database
        //    var payment = await _paymentService.GetPaymentByTransactionIdAsync(transactionId);
        //    if (payment == null)
        //    {
        //        return NotFound("Transaction not found.");
        //    }

        //    // Prepare the refund request data
        //    var refundData = new
        //    {
        //        key = MerchantKey,
        //        command = "cancel_refund_transaction",
        //        hash = GenerateRefundHash(transactionId, payment.Amount.ToString("0.00")),
        //        var1 = transactionId,
        //        var2 = payment.Amount.ToString("0.00"),
        //        var3 = reason
        //    };

        //    // Serialize refund data to JSON
        //    var jsonContent = JsonConvert.SerializeObject(refundData);
        //    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //    using (var client = new HttpClient())
        //    {
        //        try
        //        {
        //            var response = await client.PostAsync(PayURefundURL, content);
        //            var result = await response.Content.ReadAsStringAsync();

        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Log the successful refund
        //                _logger.LogInformation($"Refund successful for Transaction ID: {transactionId}");

        //                // Update the payment status in the database
        //                payment.Status = "Refunded";
        //                await _paymentService.UpdatePaymentAsync(payment);

        //                return View("RefundSuccess", payment);
        //            }
        //            else
        //            {
        //                _logger.LogError($"Refund failed for Transaction ID: {transactionId}. Response: {result}");
        //                return View("RefundFailure", result);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error occurred while processing refund.");
        //            return StatusCode(500, "An error occurred while processing the refund.");
        //        }
        //    }
        //}

        //private string GenerateRefundHash(string transactionId, string amount)
        //{
        //    string hashString = $"{MerchantKey}|cancel_refund_transaction|{transactionId}|{amount}|{MerchantSalt}";
        //    return PayUHelper.GenerateHash(hashString);
        //}
    }
}

