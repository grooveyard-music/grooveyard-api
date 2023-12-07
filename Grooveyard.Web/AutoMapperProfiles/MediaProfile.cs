using AutoMapper;
using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.Models.Media;


public class MediaProfile : Profile
{
    public MediaProfile()
    {

        CreateMap<Mix, MixDto>()
           .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));
        CreateMap<Song, SongDto>()
        .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));
        CreateMap<TracklistDto, Tracklist>()
            .ReverseMap();
    
    }
}
