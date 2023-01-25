using E_commerce_Website.Entites;
using E_commerce_Website.Entites.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Website.Data
{
    public class StoreContext : IdentityDbContext<User,Roles,int>
    {
        public StoreContext (DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<UserAddress>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Roles>()
                .HasData(
                    new Roles { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                    new Roles { Id = 2 , Name = "Admin", NormalizedName = "ADMIN" }
                );
        }
    }
}
