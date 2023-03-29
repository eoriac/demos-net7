using AutoMapper;
using Demos.API.Models.GamesDtos;

namespace Demo.API.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile() 
        {
            CreateMap<Entities.Game, GameDto>();
            CreateMap<GamesForCreationDto, Entities.Game>();
            CreateMap<GamesForUpdateDto, Entities.Game>();
            CreateMap<Entities.Game, GamesForUpdateDto>();
        }
    }
}
