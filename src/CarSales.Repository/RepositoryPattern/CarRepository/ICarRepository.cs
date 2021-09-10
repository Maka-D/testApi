using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.RepositoryPattern.CarRepository
{
    public interface ICarRepository
    {
        Task<Car> GetCar(string VinCode);
        Task<Car> InsertCar(Car entity);
        Task DeleteCar(Car entity);
        Task<bool> UpdateCar(Car entity);
        Task<IEnumerable<Car>> CarsToSale(DateTime fromDate, DateTime toDate);
    }
}
