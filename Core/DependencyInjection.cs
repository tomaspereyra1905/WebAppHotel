using System;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class DependencyInjection
    {
        
            public static IServiceCollection AgregarCore(this IServiceCollection services)
            {
                services.AddTransient<IHotelService, HotelService>();
                services.AddTransient<IRoomService, RoomService>();
                return services;
            }
        
    }
}

