using Application.Dtos;
using AutoMapper;
using Domain.Models;



namespace Application.Mappers
{
    public class MembershipProfile : Profile
    {
        public MembershipProfile()
        {
            CreateMap<Membership, MembershipDto>().ReverseMap();
        }
    }
}
