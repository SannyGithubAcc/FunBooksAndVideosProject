using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers
{
    public class OrderProductProfile : Profile
    {
        public OrderProductProfile()
        {
            CreateMap<OrderProduct, OrderProductDto>()
             .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Order_ID))
             .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product_ID))
             .ForMember(dest => dest.MembershipName, opt => opt.MapFrom(src => src.Membership.Name))
             .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
             .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<OrderProductDto, OrderProduct>()
                .ForMember(dest => dest.Order_ID, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.Product_ID, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Membership, opt => opt.MapFrom(src => new Membership { Name = src.MembershipName }))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

        }
    }
}
