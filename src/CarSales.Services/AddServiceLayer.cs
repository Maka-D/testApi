using CarSales.Services.CarServices;
using CarSales.Services.ClientServices;
using CarSales.Services.MapperService;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services
{
    public static class AddServiceLayer
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddFluentValidation()
                .AddTransient(typeof(IClientService), typeof(ClientService))
                .AddTransient(typeof(ICarService), typeof(CarService))
                .AddAutoMapper(typeof(MapperProfile));
        }
    }
}
