using AutoMapper;
using Usuarios.Domain.Entities;
using DTO = Usuarios.Application.Dto;

namespace Usuarios.Application.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, DTO.User>()
            .ConstructUsing(src => new DTO.User())
            .ReverseMap()
            .ConstructUsing(src => new User())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.RemovedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RemovedBy, opt => opt.Ignore());

        CreateMap<DTO.GuestUser, User>()
            .ConstructUsing(src => new User());
    }
}