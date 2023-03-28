using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICustomerMembershipService
    {
        Task<CustomerMembershipDto> GetByIdAsync(int id);
        Task<List<CustomerMembershipDto>> GetAllAsync();
        Task AddAsync(CustomerMembershipDto customerMembershipDto);
        Task UpdateAsync(CustomerMembershipDto customerMembershipDto);
        Task DeleteAsync(int id);
    }
}
