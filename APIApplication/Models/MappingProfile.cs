using AutoMapper;
using DataAccess.Objects;

namespace DemoAPIApplication
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.GrandParent, GrandParent>().ReverseMap();
            CreateMap<Models.Child, Child>().ReverseMap();
            CreateMap<Models.Parent, Parent>().ReverseMap();
        }
    }
}