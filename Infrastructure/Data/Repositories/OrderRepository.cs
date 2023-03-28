using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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
            await dbContext.Order.AddAsync(entity);
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

    }

}
