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
    public class ProductRepository : IRepository<Product>
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await dbContext.Product.FindAsync(id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await dbContext.Product.ToListAsync();
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await dbContext.Product.AddAsync(entity);
            return entity;
        }

        public void Update(Product entity)
        {
            dbContext.Product.Update(entity);
        }

        public void Delete(Product entity)
        {
            dbContext.Product.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

    }
}
