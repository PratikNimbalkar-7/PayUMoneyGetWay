using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace ProductDemo.Models
{
    public class ProductDB : DbContext
    {
        public ProductDB(DbContextOptions<ProductDB> options) : base(options) { }

        public DbSet<Products> Products { get; set; }

        public DbSet<CartItem> Carts { get; set; }

       public DbSet<Payments> Payments { get; set; }

    }

}
