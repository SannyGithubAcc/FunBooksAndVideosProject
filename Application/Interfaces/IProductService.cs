using Application.Dtos;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id);
        Task<List<ProductDto>> GetAllAsync();
        Task AddAsync(ProductDto productDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(int id);
    }
}
