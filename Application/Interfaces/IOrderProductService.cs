using Application.Dtos;

namespace Application.Interfaces
{
    public interface IOrderProductService
    {
        Task<IEnumerable<OrderProductDto>> GetAllOrderProductsAsync();
        Task<OrderProductDto> GetOrderProductByIdAsync(int id);
        Task<OrderProductDto> AddOrderProductAsync(OrderProductDto orderProductDto);
        Task UpdateOrderProductAsync(OrderProductDto orderProductDto);
        Task DeleteOrderProductAsync(int id);
    }
}
