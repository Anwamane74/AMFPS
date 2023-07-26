using AutoMapper;
using EFCore.Entities;
using Services.Features.UserSecurity;

namespace Services.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegistrationUseCase.UserForRegistrationCommand, User>()
            .ForMember(
                u => u.UserName, 
                opt => opt.MapFrom(
                    x => x.Email));
    }
}