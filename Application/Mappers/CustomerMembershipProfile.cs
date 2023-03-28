using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers
{
    public class CustomerMembershipProfile : Profile
    {
        public CustomerMembershipProfile()
        {
            CreateMap<CustomerMembership, CustomerMembershipDto>().ReverseMap();
        }
    }
}
