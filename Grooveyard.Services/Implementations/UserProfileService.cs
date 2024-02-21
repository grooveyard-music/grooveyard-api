
using AutoMapper;
using Azure.Storage.Blobs;
using Grooveyard.Domain.Entities;
using Grooveyard.Infrastructure.Data.Repositories;
using Grooveyard.Infrastructure.Data.Repositories.Interfaces;
using Grooveyard.Services.DTOs;
using Grooveyard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Windows.Input;

namespace Grooveyard.Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _repository;
        private readonly IPostRepository _postRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IMapper _mapper;
        public UserProfileService(IUserRepository repository, IMapper mapper, IPostRepository postRepository, IDiscussionRepository discussionRepository, IMediaRepository mediaRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _postRepository = postRepository;
            _discussionRepository = discussionRepository;
            _mediaRepository = mediaRepository;
        }

        public async Task<UserProfileDto> GetUserProfile(string userId)
        {

            var getCurrentProfile = await _repository.GetUserProfile(userId);
            var currentProfileDto = _mapper.Map<UserProfileDto>(getCurrentProfile);

            currentProfileDto.UserActivity = await GetUserCommunityActivity(userId);

            return currentProfileDto;
        }

        public async Task<List<TrackDto>> GetUserMusicboxAsync(string userId)
        {
            var musicBox = await _mediaRepository.GetOrCreateUserMusicboxAsync(userId);

            if (musicBox == null)
            {
                return new List<TrackDto>();
            }

            var trackDtos = new List<TrackDto>();
            foreach (var musicboxTrack in musicBox.MusicboxTracks)
            {
                var track = await _mediaRepository.GetTrackByIdAsync(musicboxTrack.TrackId);
                if (track != null)
                {
                    var trackDto = _mapper.Map<TrackDto>(track);
                    trackDtos.Add(trackDto);
                }
            }

            return trackDtos;
        }


        public async Task<List<TrackDto>> SearchUserMusicboxAsync(string userId, string searchTerm)
        {
            var musicBox = await _mediaRepository.SearchUserMusicboxAsync(userId, searchTerm);
            if (musicBox == null)
            {
                return new List<TrackDto>();
            }

            var trackDtos = new List<TrackDto>();
            foreach (var musicboxTrack in musicBox.MusicboxTracks)
            {
                var track = await _mediaRepository.GetTrackByIdAsync(musicboxTrack.TrackId);
                if (track != null)
                {
                    var trackDto = _mapper.Map<TrackDto>(track);
                    trackDtos.Add(trackDto);
                }
            }

            return trackDtos;
        }

        public async Task<List<UserProfileDto>> GetUserProfilesByIds(List<string> userIds)
        {
            var userProfiles = await _repository.GetUserProfilesByIds(userIds);

            var userProfilesDto = _mapper.Map<List<UserProfileDto>>(userProfiles);

            return userProfilesDto;
        }

        public async Task<UserProfile> CreateUserProfile(string userId, string userName)
        {

            var userProfile = new UserProfile
            {
                UserId = userId,
                DisplayName = userName,
                AvatarUrl = "https://grooveyarduser.blob.core.windows.net/avatars/default_avatar.jpg"
            };

            var profileCreated = await _repository.CreateUserProfile(userProfile);

            return profileCreated;
        }

        public async Task<UserProfileDto> UpdateUserProfile(UpdateUserProfileDto updatedUserProfile)
        {
            var getCurrentProfile = await _repository.GetUserProfile(updatedUserProfile.userId);

            _mapper.Map(updatedUserProfile, getCurrentProfile);

            var updatedProfile = await _repository.UpdateUserProfile(getCurrentProfile);

            var updatedProfileDto = _mapper.Map<UserProfileDto>(updatedProfile);

            return updatedProfileDto;

        }

        public async Task<UserActivityDto> GetUserCommunityActivity(string userId)
        {

            var userActivity = new UserActivityDto
            {
                postCount = await _postRepository.GetPostCountForUserAsync(userId),
                discussionCount = await _discussionRepository.GetDiscussionCountForUserAsync(userId),
                commentCount = await _postRepository.GetCommentCountForUserAsync(userId)
            };

            return userActivity;
        }

        public async Task<bool> UpdateUserProfileAvatar(string userId, IFormFile imageFile)
        {
            string uniqueBlobName = $"{userId}-avatar";
            string avatarUrl = await UploadToBlobStorage(imageFile, uniqueBlobName, "avatars");
            var userProfile = await _repository.GetUserProfile(userId);
            userProfile.AvatarUrl = avatarUrl;
            var updatedProfile = await _repository.UpdateUserProfile(userProfile);
            if(updatedProfile != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateUserProfileCover(string userId, IFormFile imageFile)
        {
            string uniqueBlobName = $"{userId}-cover";
            string coverUrl = await UploadToBlobStorage(imageFile, uniqueBlobName, "covers");
            var userProfile = await _repository.GetUserProfile(userId);
            userProfile.CoverUrl = coverUrl;
            var updatedProfile = await _repository.UpdateUserProfile(userProfile);
            if (updatedProfile != null)
            {
                return true;
            }

            return false;
        }
        private async Task<string> UploadToBlobStorage(IFormFile file, string blobName, string type)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=grooveyarduser;AccountKey=IItU/hYos9UjDJa5ztnClRbMIxWFcihcRrljNHuA9Ozi4Ncr2Rr8Mj2vagGYOtl6hzZlKypM/I+i+AStvz9hew==;EndpointSuffix=core.windows.net";
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(type);

            var blobClient = blobContainerClient.GetBlobClient(blobName);
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.AbsoluteUri; 
        }

        public async Task<string> GetUserAvatar(string userId)
        {

            var getCurrentProfile = await _repository.GetUserProfile(userId);

            return getCurrentProfile.AvatarUrl;
        }

        public async Task<bool> CheckDisplayName(string displayName)
        {

            var getCurrentProfile = await _repository.CheckDisplayName(displayName);

            return getCurrentProfile;
        }


    }


}

