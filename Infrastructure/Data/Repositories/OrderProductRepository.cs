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
    public class OrderProductRepository : IRepository<OrderProduct>
    {
        private readonly ApplicationDbContext dbContext;

        public OrderProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OrderProduct> GetByIdAsync(int id)
        {
            return await dbContext.OrderProduct.FindAsync(id);
        }

        public async Task<List<OrderProduct>> GetAllAsync()
        {
            return await dbContext.OrderProduct.ToListAsync();
        }

        public async Task<OrderProduct> AddAsync(OrderProduct entity)
        {
            await dbContext.OrderProduct.AddAsync(entity);
            return entity;
        }

        public void Update(OrderProduct entity)
        {
            dbContext.OrderProduct.Update(entity);
        }

        public void Delete(OrderProduct entity)
        {
            dbContext.OrderProduct.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }


    }
}
