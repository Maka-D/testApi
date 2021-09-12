using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
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
        Task<List<Car>> CarsToSale(DateTime fromDate, DateTime toDate);
        Task<List<ReportData>> GetCarsByMonth();
    }
}
