using System;
using Api.Models;
using AutoMapper;
using Core.Entities;


namespace Api.Dependencies
{
    public static class AutoMapperDependencyInjection
    {
        public static IServiceCollection AgregarAutoMapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            return services.AddSingleton(mapper);
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelViewModel, Hotel>().ReverseMap();
            CreateMap<RoomViewModel, Room>().ReverseMap();        
        }
    }
}

