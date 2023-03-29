using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Customer> CustomerRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<OrderProduct> OrderProductRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Membership> MembershipRepository { get; }
        IRepository<CustomerMembership> CustomerMembershipRepository { get; }
        Task SaveChangesAsync();
    }

}
