using AutoMapper;
using Grooveyard.Domain.DTO.User;
using Grooveyard.Domain.DTO.Media;
using Grooveyard.Domain.Interfaces.Services.Media;
using Grooveyard.Domain.Interfaces.Repositories.Media;

namespace Grooveyard.Services.MediaService
{
    public class MediaService : IMediaService
    {

        private readonly IMediaRepository _mediaRepository;

        private readonly IMapper _mapper;
        public MediaService(IMediaRepository mediaRepository, IMapper mapper)
        {
            _mediaRepository = mediaRepository;
            _mapper = mapper;
        }


        public async Task<UserMediaDto> GetUserMediaAsync(string userId)
        {

            var mixes = await _mediaRepository.GetUserMixesAsync(userId);
            var songs = await _mediaRepository.GetUserSongsAsync(userId);

            var mixDtos = _mapper.Map<List<MixDto>>(mixes);
            var songDtos = _mapper.Map<List<SongDto>>(songs);

            var userProfileFeed = new UserMediaDto
            {
                Mixes = mixDtos,
                Songs = songDtos
            };

            return userProfileFeed;
        }


    }

}
