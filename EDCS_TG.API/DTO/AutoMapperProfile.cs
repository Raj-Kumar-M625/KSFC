using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Dto;


public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserCreateDto>().ReverseMap();
    }
}