using Application.Dtos;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(int id);
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto> AddAsync(OrderDto orderDto);
        Task UpdateAsync(OrderDto orderDto);
        Task DeleteAsync(int id);

    }
}
