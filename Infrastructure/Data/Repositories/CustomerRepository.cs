using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Customer.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error getting customer with id {id}", ex);
            }
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                return await dbContext.Customer.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error getting all customers", ex);
            }
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            try
            {
                await dbContext.Customer.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error adding customer", ex);
            }
            return entity;

        }

        public void Update(Customer entity)
        {
            try
            {
                dbContext.Customer.Update(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error updating customer with id {entity.Id}", ex);
            }
        }

        public void Delete(Customer entity)
        {
            try
            {
                dbContext.Customer.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error deleting customer with id {entity.Id}", ex);
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
                throw new RepositoryException("Error saving changes to database", ex);
            }
        }
    }
}
