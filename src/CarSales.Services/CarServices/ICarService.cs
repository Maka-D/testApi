using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using CarSales.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.CarServices
{
    public interface ICarService
    {
        Task<Car> AddCar(string IdentityNumber, CarInput car);
        Task DeleteCar(IdentifyingData input);
        Task<bool> BuyCar(IdentifyingData input);
        Task<List<Car>> SellingCarsList(DateInput input);
        Task<List<ReportData>> MonthlyReport();
    }
}
