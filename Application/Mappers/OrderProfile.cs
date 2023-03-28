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
           .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerID))
           .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.CustomerID, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));


        }
    }
}
