using AutoMapper;

namespace Demo.API.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile() 
        {
            CreateMap<Entities.Game, Models.GameDto>();
            CreateMap<Models.GamesForCreationDto, Entities.Game>();
            CreateMap<Models.GamesForUpdateDto, Entities.Game>();
            CreateMap<Entities.Game, Models.GamesForUpdateDto>();
        }
    }
}
