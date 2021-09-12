using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using CarSales.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.CarService
{
    public interface ICarService
    {
        Task<Car> AddCar(string IdentityNumber, CarInput car);
        Task DeleteCar(string IdentityNum, string VinCode);
        Task<bool> BuyCar(string IdentityNum, string VinCode);
        Task<IEnumerable<Car>> SellingCarsList(DateTime from, DateTime to);
        Task<List<ReportData>> MonthlyReport();
    }
}
