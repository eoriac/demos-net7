﻿using AutoMapper;

namespace Demo.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<Entities.User, Models.UserDto>();
        }
    }
}
