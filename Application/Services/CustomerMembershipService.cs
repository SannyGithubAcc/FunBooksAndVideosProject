using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class CustomerMembershipService : ICustomerMembershipService
    {
        private readonly IRepository<CustomerMembership> customerMembershipRepository;
        private readonly IMapper mapper;

        public CustomerMembershipService(IRepository<CustomerMembership> customerMembershipRepository, IMapper mapper)
        {
            this.customerMembershipRepository = customerMembershipRepository;
            this.mapper = mapper;
        }

        public async Task<CustomerMembershipDto> GetByIdAsync(int id)
        {
            var customerMembership = await customerMembershipRepository.GetByIdAsync(id);
            return mapper.Map<CustomerMembershipDto>(customerMembership);
        }

        public async Task<List<CustomerMembershipDto>> GetAllAsync()
        {
            var customerMemberships = await customerMembershipRepository.GetAllAsync();
            return mapper.Map<List<CustomerMembershipDto>>(customerMemberships);
        }

        public async Task AddAsync(CustomerMembershipDto customerMembershipDto)
        {
            var customerMembership = mapper.Map<CustomerMembership>(customerMembershipDto);
            await customerMembershipRepository.AddAsync(customerMembership);
        }

        public async Task UpdateAsync(CustomerMembershipDto customerMembershipDto)
        {
            var customerMembership = mapper.Map<CustomerMembership>(customerMembershipDto);
            customerMembershipRepository.Update(customerMembership);
            await customerMembershipRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customerMembership = await customerMembershipRepository.GetByIdAsync(id);
            customerMembershipRepository.Delete(customerMembership);
            await customerMembershipRepository.SaveChangesAsync();
        }

    }
}
