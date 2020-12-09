using AutoMapper;
using uBeac.IoT.Api.DTOModels;
using uBeac.IoT.Models;

namespace uBeac.IoT.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TeamAddModel, Team>();
        }
    }
}
