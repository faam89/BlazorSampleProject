using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorPracticeApp.Shared.Entities;

namespace BlazorPracticeApp.Server.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Person, Person>().ForMember(s => s.Picture,
                option => option.Ignore());
        }
    }
}
