using Application.Dtos;

namespace Application.Interfaces
{
    public interface IMembershipService
    {
        Task<MembershipDto> GetByIdAsync(int id);
        Task<List<MembershipDto>> GetAllAsync();
        Task AddAsync(MembershipDto membershipDto);
        Task UpdateAsync(MembershipDto membershipDto);
        Task DeleteAsync(int id);
    }
}
