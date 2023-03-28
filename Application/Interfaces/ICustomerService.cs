using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetByIdAsync(int id);
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto> AddAsync(CustomerDto customerDto);
        Task UpdateAsync(CustomerDto customerDto);
        Task DeleteAsync(int id);
    }
}
