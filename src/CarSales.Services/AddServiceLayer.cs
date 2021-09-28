using CarSales.Services.CacheService;
using CarSales.Services.CarServices;
using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
using CarSales.Services.FluentValidation;
using CarSales.Services.FluentValidator;
using CarSales.Services.MapperService;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services
{
    public static class AddServiceLayer
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddFluentValidation(fv =>
                  fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                )
                .AddTransient(typeof(IClientService), typeof(ClientService))
                .AddTransient(typeof(ICarService), typeof(CarService))
                .AddAutoMapper(typeof(MapperProfile))
                .AddScoped(typeof(IValidator<ClientInput>), typeof(ClientInputValidation))
                .AddScoped(typeof(IValidator<CarInput>), typeof(CarInputValidation))
                .AddScoped(typeof(IValidator<DateInput>), typeof(DateInputValidation))
                .AddSingleton(typeof(ICacheService), typeof(RedisCacheService))
                .AddMemoryCache()
                .AddStackExchangeRedisCache(options =>
                { options.Configuration = "localhost:6379"; });
        }
    }
}
