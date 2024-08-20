using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using ProductDemo.Interface;
using ProductDemo.Models;
using System.Text;

namespace ProductDemo.Controllers
{
    public class CartController : Controller
    {
        private const string MerchantKey = "srPlMt";
        private const string MerchantSalt = "kUG7FpuDkRzDcPkZPJmBlcAa6DnceZBu";
        private const string PayUBaseURL = "https://test.payu.in/_payment";

        private readonly ICartService _cartService;
        private readonly ProductDB _productDB;
        private readonly IConfiguration _configuration;

        public CartController(ICartService cartService, ProductDB productDB, IConfiguration configuration)
        {
            _cartService = cartService;
            _productDB = productDB;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddtoCart(int Pro_Id)
        {
            var product = await GetProductById(Pro_Id);
            if (product != null)
            {
                _cartService.AddToCart(product);

            }
            _productDB.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _productDB.Products.FirstOrDefaultAsync(p => p.Pro_Id == id);
        }

        public IActionResult CheckOut(Products products)
        {
            var cartitem = _cartService.GetCartItems();
            var totalAmount = cartitem.Sum(item => item.Pro_Prize * item.Quantity);

            ViewBag.TotalAmount = totalAmount;
            ViewBag.CartItem = cartitem;
            return View(cartitem);
        }

    }
    
}
 