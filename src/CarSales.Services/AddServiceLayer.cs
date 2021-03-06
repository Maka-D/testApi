using CarSales.Services.CacheService;
using CarSales.Services.CarServices;
using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
using CarSales.Services.FluentValidation;
using CarSales.Services.FluentValidator;
using CarSales.Services.MapperService;
using CarSales.Services.TokenService;
using CarSales.Services.UserServices;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Reflection;
using System.Text;

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
                .AddTransient(typeof(IUserService), typeof(UserService))
                .AddTransient<ITokenService, JwtTokenService>()
                .AddAutoMapper(typeof(MapperProfile))
                .AddScoped(typeof(IValidator<ClientInput>), typeof(ClientInputValidation))
                .AddScoped(typeof(IValidator<CarInput>), typeof(CarInputValidation))
                .AddScoped(typeof(IValidator<DateInput>), typeof(DateInputValidation))
                .AddScoped(typeof(IValidator<IdentifyingData>), typeof(IdentifyingDataValidation))
                .AddScoped(typeof(IValidator<LogInInput>), typeof(LogInInputValidation))
                .AddScoped(typeof(IValidator<RegistrationInput>), typeof(RegistrationInputValidation))
                .AddSingleton(typeof(ICacheService), typeof(RedisCacheService))
                .AddMemoryCache()
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = "localhost:6379";
                });
        }
    }
}
