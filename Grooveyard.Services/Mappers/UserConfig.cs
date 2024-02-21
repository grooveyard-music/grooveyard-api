using AutoMapper;
using Grooveyard.Services.DTOs;

public class UserConfig : Profile
{
    public UserConfig()
    {
        CreateMap<UserProfileDto, Grooveyard.Domain.Entities.UserProfile>()
            .ReverseMap();

        CreateMap<UpdateUserProfileDto, Grooveyard.Domain.Entities.UserProfile>()
            .ReverseMap();

    
    }
}
