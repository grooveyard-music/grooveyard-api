using AutoMapper;
using Grooveyard.Domain.Entities;
using Grooveyard.Services.DTOs;

public class MediaConfig : Profile
{
    public MediaConfig()
    {

        CreateMap<Mix, MixDto>()
           .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));
        CreateMap<Song, SongDto>()
        .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));
        CreateMap<TracklistDto, Tracklist>()
            .ReverseMap();
        CreateMap<Track, TrackDto>()
            .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => src.Songs))
            .ForMember(dest => dest.Mixes, opt => opt.MapFrom(src => src.Mixes));

    }
}
