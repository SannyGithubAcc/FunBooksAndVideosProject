using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;



namespace Application.Services
{
      public class OrderProductService : IOrderProductService
    {
        private readonly IMapper mapper;
        private readonly IRepository<OrderProduct> orderProductRepository;

        public OrderProductService(IMapper mapper, IRepository<OrderProduct> orderProductRepository)
        {
            this.mapper = mapper;
            this.orderProductRepository = orderProductRepository;
        }

        public async Task<IEnumerable<OrderProductDto>> GetAllOrderProductsAsync()
        {
            var orderProducts = await orderProductRepository.GetAllAsync();
            var orderProductDtos = mapper.Map<IEnumerable<OrderProductDto>>(orderProducts);
            return orderProductDtos;
        }

        public async Task<OrderProductDto> GetOrderProductByIdAsync(int id)
        {
            var orderProduct = await orderProductRepository.GetByIdAsync(id);
            var orderProductDto = mapper.Map<OrderProductDto>(orderProduct);
            return orderProductDto;
        }


        public async Task<OrderProductDto> AddOrderProductAsync(OrderProductDto orderProductDto)
        {
            var orderProduct = mapper.Map<OrderProduct>(orderProductDto);
            var addOrderProductDto = await orderProductRepository.AddAsync(orderProduct);
            await orderProductRepository.SaveChangesAsync();
            var res = mapper.Map<OrderProductDto>(addOrderProductDto);
            return res;
        }

        public async Task UpdateOrderProductAsync(OrderProductDto orderProductDto)
        {
            var orderProduct = mapper.Map<OrderProduct>(orderProductDto);
            orderProductRepository.Update(orderProduct);
            await orderProductRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderProductAsync(int id)
        {
            var orderProduct = await orderProductRepository.GetByIdAsync(id);
            orderProductRepository.Delete(orderProduct);
            await orderProductRepository.SaveChangesAsync();
        }
    }

}
