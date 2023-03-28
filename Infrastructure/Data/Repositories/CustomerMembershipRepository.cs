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
    public class CustomerMembershipRepository : IRepository<CustomerMembership>
    {
        private readonly ApplicationDbContext dbContext;

        public CustomerMembershipRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CustomerMembership> GetByIdAsync(int id)
        {
            return await dbContext.CustomerMembership.FindAsync(id);
        }

        public async Task<List<CustomerMembership>> GetAllAsync()
        {
            return await dbContext.CustomerMembership.ToListAsync();
        }

        public async Task<CustomerMembership> AddAsync(CustomerMembership entity)
        {
            await dbContext.CustomerMembership.AddAsync(entity);
            return entity;
        }

        public void Update(CustomerMembership entity)
        {
            dbContext.CustomerMembership.Update(entity);
        }

        public void Delete(CustomerMembership entity)
        {
            dbContext.CustomerMembership.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

    }
}
