using AutoMapper;
using Grooveyard.Domain.DTO.User;



public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserProfileDto, Grooveyard.Domain.Models.User.UserProfile>()
            .ReverseMap();

        CreateMap<UpdateUserProfileDto, Grooveyard.Domain.Models.User.UserProfile>()
            .ReverseMap();

    
    }
}
