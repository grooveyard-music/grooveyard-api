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
            if (string.IsNullOrEmpty(trackDto.UrlPath))
            {
                throw new ArgumentException("A URL must be provided.");
            }

            Track existingTrack = null;

            if (trackDto.Type.Equals("Song", StringComparison.OrdinalIgnoreCase))
            {
                var existingSong = await _mediaRepository.GetSongByUrlPath(ExtractYoutubeVideoId(trackDto.UrlPath));
                if (existingSong != null)
                {
                    existingTrack = await _mediaRepository.GetTrackBySongId(existingSong.Id);
                }
            }
            else if (trackDto.Type.Equals("Mix", StringComparison.OrdinalIgnoreCase))
            {
                var existingMix = await _mediaRepository.GetMixByUrlPath(ExtractYoutubeVideoId(trackDto.UrlPath));
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
                    Type = trackDto.Type,
                    DateCreated = DateTime.Now
                };

                if (trackDto.Type.Equals("Song", StringComparison.OrdinalIgnoreCase))
                {
                    Song newSong = await CreateSongEntity(trackDto, userId);
                    await _uploadRepository.CreateSongAsync(newSong);
                    existingTrack.Song = newSong;
                }
                else
                {
                    Mix newMix = await CreateMixEntity(trackDto, userId);
                    await _uploadRepository.CreateMixAsync(newMix);
                    existingTrack.Mix = newMix;
                }

                await _uploadRepository.UploadTrackAsync(existingTrack);
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
                UrlPath = ExtractYoutubeVideoId(trackDto.UrlPath),
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
                UrlPath = ExtractYoutubeVideoId(trackDto.UrlPath),
                Genres = await _uploadRepository.GetGenresByNamesAsync(trackDto.Genres)
            };

            return newMix;
        }
        private async Task<string> UploadToBlobStorage(IFormFile file, string uniqueBlobName, string containerName)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=grooveyarduser;AccountKey=IItU/hYos9UjDJa5ztnClRbMIxWFcihcRrljNHuA9Ozi4Ncr2Rr8Mj2vagGYOtl6hzZlKypM/I+i+AStvz9hew==;EndpointSuffix=core.windows.net";
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(uniqueBlobName);
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.AbsoluteUri;
        }



        public static string? ExtractYoutubeVideoId(string url)
        {
            var youtubeMatch = Regex.Match(url, @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            return youtubeMatch.Success ? youtubeMatch.Groups[1].Value : null;
        }




    }

}
