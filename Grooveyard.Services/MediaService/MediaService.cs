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


    }

}
