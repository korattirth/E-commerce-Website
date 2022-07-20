using E_commerce_Website.Entites;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Website.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext (DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
