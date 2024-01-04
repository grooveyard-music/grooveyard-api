using AutoMapper;
using Grooveyard.Domain.DTO.Social;
using Grooveyard.Domain.Models.Social;

public class SocialProfile : Profile
{
    public SocialProfile()
    {
        CreateMap<CreatePostDto, Post>()
                   .ReverseMap();
        CreateMap<PostDto, Post>()
                  .ReverseMap();
        CreateMap<CreateCommentDto, Comment>()
                  .ReverseMap();
        CreateMap<CreateDiscussionDto, Discussion>()
                  .ReverseMap();

    }
}
