using AutoMapper;
using Azure.Storage.Blobs;
using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.Interfaces.Repositories.Media;
using Grooveyard.Domain.Interfaces.Services.Media;
using Grooveyard.Domain.Models.Media;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using static Grooveyard.Domain.Models.Media.Mix;


namespace Grooveyard.Services.MediaService
{

    public class UploadService : IUploadService
    {

        private readonly IUploadRepository _uploadRepository;
        private readonly IMapper _mapper;
        public UploadService(IUploadRepository uploadRepository, IMapper mapper)
        {
            _uploadRepository = uploadRepository;
            _mapper = mapper;
        }

        public async Task<TracklistDto> UploadTracklist(TracklistDto tracklist)
        {

            return tracklist;
        }
        public async Task<SongDto> AddSongToTracklist(string tracklistId, string songId)
        {


            return new SongDto();
        }
        public async Task<MixDto> UploadMix(UploadMixDto mixDto, string userId)
        {
            if (string.IsNullOrEmpty(mixDto.UrlPath) && mixDto.MusicFile == null)
            {
                throw new ArgumentException("Either a URL or a File must be provided.");
            }

            if (!string.IsNullOrEmpty(mixDto.UrlPath) && mixDto.MusicFile != null)
            {
                throw new ArgumentException("Both URL and File cannot be provided. Choose one.");
            }

            Mix newMix = new Mix
            {
                Id = Guid.NewGuid().ToString(),
                Title = mixDto.Title,
                Artist = mixDto.Artist,
                Duration = mixDto.Duration,
                CreatedAt = DateTime.Now,
                UserId = userId,
                Host = mixDto.Host,
                Genres = await _uploadRepository.GetGenresByNamesAsync(mixDto.Genres)
            };

            // If a URL is provided
            if (!string.IsNullOrEmpty(mixDto.UrlPath) && mixDto.Host == HostType.YouTube)
            {
                // If the host is of type Youtube, perform the check for existing URL.
                var videoId = ExtractYoutubeVideoId(mixDto.UrlPath); // Assuming you have a method to extract the video ID
                var existingMix = await _uploadRepository.GetMixByVideoIdAsync(videoId); // Adapt this line based on how your repository works
                if (existingMix != null)
                {
                    throw new ArgumentException("A mix with the same YouTube video ID already exists.");
                }
                else
                {
                    newMix.UrlPath = videoId;
                }
            }
            else // If a File is provided
            {
                string uniqueBlobName = $"{userId}-{mixDto.Title}";
                var containerName = "mixes";
                var blobUri = await UploadToBlobStorage(mixDto.MusicFile, uniqueBlobName, containerName);

                // Create a MusicFile entity
                MusicFile newMusicFile = new MusicFile
                {
                    FileName = mixDto.MusicFile.FileName,
                    FilePath = blobUri,
                    Size = mixDto.MusicFile.Length,
                    Format = mixDto.MusicFile.ContentType,
                    CreationTime = DateTime.Now
                };

                // Save MusicFile to DB
                var newMusicFileAdded = await _uploadRepository.UploadMusicFileAsync(newMusicFile);


                // Associate with Mix
                newMix.MusicFileId = newMusicFileAdded.Id;
            }

            // Save Mix to DB
            var newMixAdded = await _uploadRepository.UploadMixAsync(newMix);

            // Convert to DTO for response
            MixDto newMixDto = _mapper.Map<MixDto>(newMixAdded);

            foreach (var genre in newMix.Genres)
            {
                newMixDto.Genres.Add(genre.Name);
            }

            return newMixDto;
        }
        public async Task<SongDto> UploadSong(UploadSongDto songDto, string userId)
        {
            if (string.IsNullOrEmpty(songDto.UrlPath) && songDto.MusicFile == null)
            {
                throw new ArgumentException("Either a URL or a File must be provided.");
            }

            if (!string.IsNullOrEmpty(songDto.UrlPath) && songDto.MusicFile != null)
            {
                throw new ArgumentException("Both URL and File cannot be provided. Choose one.");
            }

            Song newSong = new Song
            {
                Id = Guid.NewGuid().ToString(),
                Title = songDto.Title,
                Artist = songDto.Artist,
                Duration = songDto.Duration,
                CreatedAt = DateTime.Now,
                UserId = userId,
                Host = songDto.Host,
                Genres = await _uploadRepository.GetGenresByNamesAsync(songDto.Genres)
            };

            // If a URL is provided
            if (!string.IsNullOrEmpty(songDto.UrlPath) && songDto.Host == HostType.YouTube)
            {
                // If the host is of type Youtube, perform the check for existing URL.
                var videoId = ExtractYoutubeVideoId(songDto.UrlPath); // Assuming you have a method to extract the video ID
                var existingSong = await _uploadRepository.GetMixByVideoIdAsync(videoId); // Adapt this line based on how your repository works
                if (existingSong != null)
                {
                    throw new ArgumentException("A mix with the same YouTube video ID already exists.");
                }
                else
                {
                    newSong.UrlPath = videoId;
                }
            }
            else // If a File is provided
            {
                string uniqueBlobName = $"{userId}-{songDto.Title}";
                string containerName = "songs";
                var blobUri = await UploadToBlobStorage(songDto.MusicFile, uniqueBlobName, containerName);

                // Create a MusicFile entity
                MusicFile newMusicFile = new MusicFile
                {
                    FileName = songDto.MusicFile.FileName,
                    FilePath = blobUri,
                    Size = songDto.MusicFile.Length,
                    Format = songDto.MusicFile.ContentType,
                    CreationTime = DateTime.Now
                };

                // Save MusicFile to DB
                var newMusicFileAdded = await _uploadRepository.UploadMusicFileAsync(newMusicFile);


                // Associate with Mix
                newSong.MusicFileId = newMusicFileAdded.Id;
            }

            // Save Mix to DB
            var newSongAdded = await _uploadRepository.UploadSongAsync(newSong);

            // Convert to DTO for response
            SongDto newSongDto = _mapper.Map<SongDto>(newSongAdded);

            foreach (var genre in newSong.Genres)
            {
                newSongDto.Genres.Add(genre.Name);
            }

            return newSongDto;
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
