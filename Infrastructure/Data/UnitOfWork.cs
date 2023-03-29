using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            CustomerRepository = new Repository<Customer>(dbContext);
            OrderRepository    = new Repository<Order>(dbContext);
            OrderProductRepository = new Repository<OrderProduct>(dbContext);
            ProductRepository = new Repository<Product>(dbContext);
            MembershipRepository = new Repository<Membership>(dbContext);
            CustomerMembershipRepository = new Repository<CustomerMembership>(dbContext);
        }

        public IRepository<Customer> CustomerRepository { get; }

        public IRepository<Order> OrderRepository { get; }

        public IRepository<OrderProduct> OrderProductRepository { get; }

        public IRepository<Product> ProductRepository { get; }

        public IRepository<Membership> MembershipRepository { get; }

        public IRepository<CustomerMembership> CustomerMembershipRepository { get; }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }

}
