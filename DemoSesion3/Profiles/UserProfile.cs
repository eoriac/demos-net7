using AutoMapper;

namespace DemoSesion3.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<Entities.User, Models.UserDto>();
        }
    }
}
