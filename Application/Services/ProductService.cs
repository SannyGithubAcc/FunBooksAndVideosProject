using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> productRepository;
        private readonly IMapper mapper;

        public ProductService(IRepository<Product> productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            return mapper.Map<ProductDto>(product);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await productRepository.GetAllAsync();
            return mapper.Map<List<ProductDto>>(products);
        }

        public async Task AddAsync(ProductDto productDto)
        {
            var product = mapper.Map<Product>(productDto);
            await productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            var product = mapper.Map<Product>(productDto);
            productRepository.Update(product);
            await productRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            productRepository.Delete(product);
            await productRepository.SaveChangesAsync();
        }

    }
 }
