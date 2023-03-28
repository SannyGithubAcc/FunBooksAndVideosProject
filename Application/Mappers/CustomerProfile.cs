using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace FunBooksAndVideosAPI.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}
