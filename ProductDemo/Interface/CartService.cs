using Newtonsoft.Json;
using ProductDemo.Models;

namespace ProductDemo.Interface
{
    public class CartService : ICartService
    {
        private readonly ProductDB _productDb;
        private readonly ISession _session;

        private const string CartSessionKey = "CartItems";

        public CartService(IHttpContextAccessor httpContextAccessor, ProductDB productDb)
        {
            _session = httpContextAccessor.HttpContext.Session;
            _productDb = productDb;
        }
        public void AddToCart(Products product)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(x => x.Pro_Id == product.Pro_Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var newItem = new CartItem
                {
                    Pro_Id = product.Pro_Id,
                    Pro_Name = product.Pro_Name,
                    Pro_Prize = product.Pro_Prize,
                    Quantity = 1
                };
                cartItems.Add(newItem);
                _productDb.Carts.Add(newItem);
            }

            _productDb.SaveChanges();
            _session.SetString(CartSessionKey, JsonConvert.SerializeObject(cartItems));
        }

        public void ClearCart()
        {
            _session.Remove(CartSessionKey);
        }

        public List<CartItem> GetCartItems()
        {
            var CartDate = _session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(CartDate))
            {
                return new List<CartItem>();
            }
            return JsonConvert.DeserializeObject<List<CartItem>>(CartDate);
        }
    }
}
