
using ProductDemo.Models;

namespace ProductDemo.Interface
{
    public interface ICartService
    {
        void AddToCart(Products product);
        List<CartItem> GetCartItems();
        void ClearCart();
    }
}
