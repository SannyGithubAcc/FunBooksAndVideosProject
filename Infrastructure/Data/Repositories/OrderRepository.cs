using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IRepository<CustomerMembership> customerMembershipRepository;

        public OrderRepository(ApplicationDbContext dbContext, IRepository<CustomerMembership> customerMembershipRepository)
        {
            this.dbContext = dbContext;
            this.customerMembershipRepository = customerMembershipRepository;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await dbContext.Order.Include(o => o.OrderProducts)
                   .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await dbContext.Order.Include(o => o.OrderProducts)
                    .ToListAsync();
        }


        public async Task<Order> AddAsync(Order entity)
        {
            ProcessOrder(entity);
            await dbContext.Order.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }


        public void Update(Order entity)
        {
            dbContext.Order.Update(entity);
        }

        public void Delete(Order entity)
        {
            dbContext.Order.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void ProcessOrder(Order order)
        {
            // TODO: Add logic to process the order.

            if (order.OrderProducts.Any(op => op.Membership.Name != null))
            {
                var customerMembership = new CustomerMembership
                {
                    CustomerId = order.Customer_ID,
                    IsActive = true
                };

                customerMembershipRepository.AddAsync(customerMembership);
            }

            if (order.OrderProducts.Any(op => op.Product.Category == "Physical"))
            {
                // TODO: Generate a shipping slip for the order.
            }

        }

    }

}
