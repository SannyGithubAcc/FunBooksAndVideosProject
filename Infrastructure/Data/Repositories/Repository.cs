using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            if (this.dbContext == null)
            {
                throw new Exception("dbContext is null.");
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("Error while getting entity by id.", ex);
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("Error while getting all entities.", ex);
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await dbContext.Set<T>().AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("Error while adding entity.", ex);
            }
        }

        public void Update(T entity)
        {
            try
            {
                dbContext.Set<T>().Update(entity);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("Error while updating entity.", ex);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                dbContext.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("Error while deleting entity.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("Error while saving changes.", ex);
            }
        }
    }


}
