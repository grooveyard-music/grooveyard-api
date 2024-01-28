using AutoMapper;
using Azure.Storage.Blobs;
using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.Interfaces.Repositories.Media;
using Grooveyard.Domain.Interfaces.Services.Media;
using Grooveyard.Domain.Models.Media;
using Grooveyard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using static Grooveyard.Domain.Models.Media.Mix;


namespace Grooveyard.Services.MediaService
{

    public class UploadService : IUploadService
    {

        private readonly IUploadRepository _uploadRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMapper _mapper;
        public UploadService(IUploadRepository uploadRepository, IMapper mapper, IMediaRepository mediaRepository)
        {
            _uploadRepository = uploadRepository;
            _mapper = mapper;
            _mediaRepository = mediaRepository;
        }

        public async Task<TracklistDto> UploadTracklist(TracklistDto tracklist)
        {

            return tracklist;
        }
        public async Task<SongDto> AddSongToTracklist(string tracklistId, string songId)
        {


            return new SongDto();
        }

        public async Task<TrackDto> UploadTrack(UploadTrackDto trackDto, string userId)
        {
            // Initial checks for URL
            if (string.IsNullOrEmpty(trackDto.Uri))
            {
                throw new ArgumentException("A URL must be provided.");
            }

            Track existingTrack = null;

            string uri = trackDto.Host switch
            {
                HostType.YouTube => ExtractYoutubeVideoId(trackDto.Uri),
                _ => trackDto.Uri,
            };

            trackDto.Uri = uri;

            if (trackDto.Type.Equals("Song", StringComparison.OrdinalIgnoreCase))
            {
                var existingSong = await _mediaRepository.GetSongByUri(trackDto.Uri);
                if (existingSong != null)
                {
                    existingTrack = await _mediaRepository.GetTrackBySongId(existingSong.Id);
                }
            }
            else if (trackDto.Type.Equals("Mix", StringComparison.OrdinalIgnoreCase))
            {
                var existingMix = await _mediaRepository.GetMixByUri(trackDto.Uri);
                if (existingMix != null)
                {
                    existingTrack = await _mediaRepository.GetTrackByMixId(existingMix.Id);
                }
            }
            else
            {
                throw new ArgumentException("Invalid track type. Must be 'Song' or 'Mix'.");
            }

            if (existingTrack == null)
            {
                existingTrack = new Track
                {
                    Id = Guid.NewGuid().ToString(),
                    DateCreated = DateTime.Now
                };

                var trackAdded = await _uploadRepository.UploadTrackAsync(existingTrack);

                if (trackDto.Type.Equals("Song", StringComparison.OrdinalIgnoreCase))
                {
                    Song newSong = await CreateSongEntity(trackDto, userId);
                    newSong.TrackId = trackAdded.Id;
                    newSong.Track = trackAdded;
                    await _uploadRepository.CreateSongAsync(newSong);
                    existingTrack.Songs.Add(newSong);
                }
                else
                {
                    Mix newMix = await CreateMixEntity(trackDto, userId);
                    await _uploadRepository.CreateMixAsync(newMix);
                    existingTrack.Mixes.Add(newMix);
                }

  
            }

            var userMusicBox = await _mediaRepository.GetOrCreateUserMusicboxAsync(userId);
            await _mediaRepository.AddTrackToMusicBox(existingTrack, userMusicBox.Id);

            TrackDto newTrackDto = _mapper.Map<TrackDto>(existingTrack);

            return newTrackDto;
        }

        // Helper methods to create Song and Mix entities
        private async Task<Song> CreateSongEntity(UploadTrackDto trackDto, string userId)
        {
            Song newSong = new Song
            {
                Id = Guid.NewGuid().ToString(),
                Title = trackDto.Title,
                Artist = trackDto.Artist,
                Duration = trackDto.Duration,
                CreatedAt = DateTime.Now,
                UserId = userId,
                Host = trackDto.Host,
                Uri = trackDto.Uri,
                Genres = await _uploadRepository.GetGenresByNamesAsync(trackDto.Genres)
            };

            return newSong;
        }

        private async Task<Mix> CreateMixEntity(UploadTrackDto trackDto, string userId)
        {
            Mix newMix = new Mix
            {
                Id = Guid.NewGuid().ToString(),
                Title = trackDto.Title,
                Artist = trackDto.Artist,
                Duration = trackDto.Duration,
                CreatedAt = DateTime.Now,
                UserId = userId,
                Host = trackDto.Host,
                Uri = trackDto.Uri,
                Genres = await _uploadRepository.GetGenresByNamesAsync(trackDto.Genres)
            };

            return newMix;
        }



        public static string? ExtractYoutubeVideoId(string url)
        {
            var youtubeMatch = Regex.Match(url, @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            return youtubeMatch.Success ? youtubeMatch.Groups[1].Value : null;
        }




    }

}
