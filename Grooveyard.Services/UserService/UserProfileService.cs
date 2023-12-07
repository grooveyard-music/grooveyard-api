
using AutoMapper;
using Azure.Storage.Blobs;
using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.Interfaces.Repositories.User;
using Grooveyard.Domain.Interfaces.Services.User;
using Grooveyard.Domain.Models.User;
using Microsoft.AspNetCore.Http;

namespace Grooveyard.Services.UserService
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        public UserProfileService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> GetUserProfile(string userId)
        {

            var getCurrentProfile = await _repository.GetUserProfile(userId);
            var currentProfileDto = _mapper.Map<UserProfileDto>(getCurrentProfile);

            return currentProfileDto;
        }

        public async Task<List<UserProfileDto>> GetUserProfilesByIds(List<string> userIds)
        {
            var userProfiles = await _repository.GetUserProfilesByIds(userIds);

            var userProfilesDto = _mapper.Map<List<UserProfileDto>>(userProfiles);

            return userProfilesDto;
        }

        public async Task<UserProfile> CreateUserProfile(string userId)
        {

            var userProfile = new UserProfile
            {
                UserId = userId,
            };

            var profileCreated = await _repository.CreateUserProfile(userProfile);

            return profileCreated;
        }

        public async Task<UserProfileDto> UpdateUserProfile(UpdateUserProfileDto updatedUserProfile)
        {
            var getCurrentProfile = await _repository.GetUserProfile(updatedUserProfile.userId);

            if (updatedUserProfile.AvatarFile != null && updatedUserProfile.AvatarFile.Length > 0)
            {
                string uniqueBlobName = $"{getCurrentProfile.UserId}-avatar";
                string avatarUrl = await UploadToBlobStorage(updatedUserProfile.AvatarFile, uniqueBlobName);
                getCurrentProfile.AvatarUrl = avatarUrl;
            }

            _mapper.Map(updatedUserProfile, getCurrentProfile);

            var updatedProfile = await _repository.UpdateUserProfile(getCurrentProfile);

            var updatedProfileDto = _mapper.Map<UserProfileDto>(updatedProfile);

            return updatedProfileDto;

        }

        private async Task<string> UploadToBlobStorage(IFormFile file, string blobName)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=grooveyarduser;AccountKey=IItU/hYos9UjDJa5ztnClRbMIxWFcihcRrljNHuA9Ozi4Ncr2Rr8Mj2vagGYOtl6hzZlKypM/I+i+AStvz9hew==;EndpointSuffix=core.windows.net";
            string containerName = "avatars";
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(blobName); // Using the unique blob name
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.AbsoluteUri;  // Return the URL of the uploaded blob
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

