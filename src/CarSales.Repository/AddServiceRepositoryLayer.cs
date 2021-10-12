using CarSales.Domain.Models;
using CarSales.Repository.CustomRepositories;
using CarSales.Repository.RepositoryPattern;
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
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<Client>), typeof(Repository<Client>))
                .AddScoped(typeof(IRepository<Car>), typeof(Repository<Car>))
                .AddTransient(typeof(ClientRepository))
                .AddTransient(typeof(UserRepository))
                .AddTransient(typeof(CarRepository));
        }
    }
}
