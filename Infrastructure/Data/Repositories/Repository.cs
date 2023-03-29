using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }


}
