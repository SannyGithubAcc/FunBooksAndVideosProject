using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;



namespace Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IRepository<Membership> membershipRepository;
        private readonly IMapper mapper;

        public MembershipService(IRepository<Membership> membershipRepository, IMapper mapper)
        {
            this.membershipRepository = membershipRepository;
            this.mapper = mapper;
        }

        public async Task<MembershipDto> GetByIdAsync(int id)
        {
            var membership = await membershipRepository.GetByIdAsync(id);
            return mapper.Map<MembershipDto>(membership);
        }

        public async Task<List<MembershipDto>> GetAllAsync()
        {
            var memberships = await membershipRepository.GetAllAsync();
            return mapper.Map<List<MembershipDto>>(memberships);
        }

        public async Task AddAsync(MembershipDto membershipDto)
        {
            var membership = mapper.Map<Membership>(membershipDto);
            await membershipRepository.AddAsync(membership);
        }

        public async Task UpdateAsync(MembershipDto membershipDto)
        {
            var membership = mapper.Map<Membership>(membershipDto);
            membershipRepository.Update(membership);
            await membershipRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var membership = await membershipRepository.GetByIdAsync(id);
            membershipRepository.Delete(membership);
            await membershipRepository.SaveChangesAsync();
        }

    }
}
