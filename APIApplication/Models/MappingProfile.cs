using AutoMapper;
using DataAccess.Objects;

namespace DemoAPIApplication
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.GrandParent, GrandParent>();
            CreateMap<Models.Child, Child>();
            CreateMap<Models.Parent, Parent>();
        }
    }
}