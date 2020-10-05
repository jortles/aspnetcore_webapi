using aspnetcore_webapi.Models;
using aspnetcore_webapi.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
