using AutoMapper;
using Grooveyard.Domain.Entities;
using Grooveyard.Services.DTOs;

public class SocialConfig : Profile
{
    public SocialConfig()
    {
        CreateMap<CreatePostDto, Post>()
                   .ReverseMap();
        CreateMap<PostDto, Post>()
                  .ReverseMap();
        CreateMap<Discussion, DiscussionDto>()
           .ForMember(
               dest => dest.Genres,
               opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList())
           )
           .ReverseMap();
        CreateMap<LikeDto, Like>()
                  .ReverseMap();
        CreateMap<CreateCommentDto, Comment>()
                  .ReverseMap();
        CreateMap<CreateDiscussionDto, Discussion>()
                  .ReverseMap();

    }
}
