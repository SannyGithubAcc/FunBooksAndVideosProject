using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
        public DbSet<Membership> Membership { get; set; }
        public DbSet<CustomerMembership> CustomerMembership { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
              .HasMany(o => o.OrderProducts)
              .WithOne(op => op.Order)
              .HasForeignKey(op => op.OrderID)
              .OnDelete(DeleteBehavior.Cascade);
        }
        
    }

}
