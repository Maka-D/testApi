using CarSales.Domain.Models;
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
        Task<Car> GetCar(string VinCode);
        //Task UpdateCar(CarInput client);
        //Task<Car> GetCar(string VinCode);
        Task DeleteCar(string IdentityNum, string VinCode);
    }
}
