using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
           .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer_ID))
           .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.Customer_ID, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));
        }
    }
}
