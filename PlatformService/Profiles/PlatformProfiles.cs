using AutoMapper;

namespace PlatformService.Profiles
{
    public class PlatformProfiles : Profile
    {
        public PlatformProfiles()
        {
           CreateMap<Models.Platform, Models.DTOs.PlatformDTO>();
            CreateMap<Models.DTOs.PlatformCreateDTO, Models.Platform>();
            CreateMap<Models.DTOs.PlatformDTO,Models.DTOs.PlatformPublishedDto>();
        }
    }
}
