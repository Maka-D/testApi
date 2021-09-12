using CarSales.Repository.RepositoryPattern.CarRepository;
using CarSales.Repository.RepositoryPattern.ClientRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository
{
    public static class AddServiceRepositoryLayer 
    {
        public static IServiceCollection AddServices(this IServiceCollection services, string conString)
        {
            return services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(conString))
                .AddScoped(typeof(IClientRepository), typeof(ClientRepository))
                .AddScoped(typeof(ICarRepository), typeof(CarRepository));
        }
    }
}
