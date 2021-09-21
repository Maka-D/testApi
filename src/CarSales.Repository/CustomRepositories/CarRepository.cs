using CarSales.Domain.Models;
using CarSales.Repository.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.CustomRepositories
{
    public class CarRepository :Repository<Car>
    {
        private readonly AppDbContext _appDbContext;
        public CarRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public new async Task<List<Car>> GetByCondition(Func<Car, bool> predicate)
        {

            var  entities =  _appDbContext.Cars.Include("Client").Where((Func<Car, bool>)predicate).ToList();

            return await Task.FromResult(entities.ToList());
        }
    }
}
