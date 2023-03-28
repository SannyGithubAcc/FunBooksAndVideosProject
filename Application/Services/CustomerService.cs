using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> customerRepository;
        private readonly IMapper mapper;

        public CustomerService(IRepository<Customer> customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await customerRepository.GetAllAsync();
            return mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);

            var addedCustomer = await customerRepository.AddAsync(customer);
            await customerRepository.SaveChangesAsync();  
            return mapper.Map<CustomerDto>(addedCustomer);
        }

        public async Task UpdateAsync(CustomerDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);
            customerRepository.Update(customer);
            await customerRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            customerRepository.Delete(customer);
            await customerRepository.SaveChangesAsync();
        }

    }
}
