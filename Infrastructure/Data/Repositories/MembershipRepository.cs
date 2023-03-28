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
    public class MembershipRepository : IRepository<Membership>
    {
        private readonly ApplicationDbContext dbContext;

        public MembershipRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Membership> GetByIdAsync(int id)
        {
            return await dbContext.Membership.FindAsync(id);
        }

        public async Task<List<Membership>> GetAllAsync()
        {
            return await dbContext.Membership.ToListAsync();
        }

        public async Task<Membership> AddAsync(Membership entity)
        {
            await dbContext.Membership.AddAsync(entity);
            return entity;
        }

        public void Update(Membership entity)
        {
            dbContext.Membership.Update(entity);
        }

        public void Delete(Membership entity)
        {
            dbContext.Membership.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

    }
}
