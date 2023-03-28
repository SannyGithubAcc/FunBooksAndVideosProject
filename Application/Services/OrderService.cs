using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;


namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IMapper mapper;

        public OrderService(IRepository<Order> orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            return mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await orderRepository.GetAllAsync();
            return mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> AddAsync(OrderDto orderDto)
        {
            var order = mapper.Map<Order>(orderDto);
            var totalPrice = 0m;
            foreach (var orderProduct in order.OrderProducts)
            {
                if (orderProduct.Product != null)
                {
                    totalPrice += orderProduct.Quantity * orderProduct.Product.price;
                }
                else if (orderProduct.Membership != null)
                {
                    totalPrice += orderProduct.Quantity * orderProduct.Membership.Price;
                }
            }
            order.price = totalPrice;
            var addedOrder =  await orderRepository.AddAsync(order);
            await orderRepository.SaveChangesAsync();
            return mapper.Map<OrderDto>(addedOrder);

        }

        public async Task UpdateAsync(OrderDto orderDto)
        {
            var order = mapper.Map<Order>(orderDto);
            orderRepository.Update(order);
            await orderRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            orderRepository.Delete(order);
            await orderRepository.SaveChangesAsync();
        }

    }
}
