using AutoMapper;

namespace DemoAPIApplication
{
    public class Utility
    {
        public Utility()
        {
            
        }
        public IMapper GetAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return config.CreateMapper();
        }
    }
}