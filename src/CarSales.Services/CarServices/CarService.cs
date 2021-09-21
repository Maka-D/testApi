using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using CarSales.Repository;
using CarSales.Repository.MemoryCacheService;
using CarSales.Repository.RepositoryPattern;
using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
using CarSales.Services.ValidateInput;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.CarServices
{
    public class CarService : ICarService
    {
        private readonly IRepository<Car> _carRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public CarService(IRepository<Car> carRepository, IMapper mapper
            ,IRepository<Client> clientRepository, ICacheService cacheService)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
            _cacheService = cacheService;
        }
        public async Task<Car> AddCar(string IdentityNumber, CarInput car)
        {
            if (!InputValidator.IsValidIdentityNumber(IdentityNumber))
            {
                throw new InvalidInputException();
            }
            var client = await _clientRepository.Get(x => x.IdentityNumber == IdentityNumber && x.DeletedAt == null);

            
            if (await _carRepository.Get(x => x.VinCode == car.VinCode && x.DeletedAt == null) != null)
                throw new CarAlreadyExistsException();

            var insertedCar = _mapper.Map<Car>(car);
            insertedCar.ClientId = client.Id;
            insertedCar.Client = client;

            if (await _carRepository.Get(x => x.VinCode == car.VinCode && x.DeletedAt != null && x.Client.IdentityNumber == IdentityNumber) != null)
            {
                return await _carRepository.Update(insertedCar);
            }          

            return await _carRepository.Insert(insertedCar);
        }

        public async Task<bool> BuyCar(string IdentityNum, string VinCode)
        {
            if (!InputValidator.IsValidIdentityNumber(IdentityNum) || !InputValidator.IsValidVinCode(VinCode))
            {
                throw new InvalidInputException();
            }

            var client = await _clientRepository.Get(x => x.IdentityNumber == IdentityNum && x.DeletedAt == null);
            var car = await _carRepository.Get(x => x.VinCode == VinCode && x.DeletedAt == null);
            if(client.Id == car.ClientId || car.IsSold == true)
            {
                throw new CouldNotBuyCarException();
            }
            car.IsSold = true;
            car.FinishedSale = DateTime.Now;
            await _carRepository.Update(car);
            if (_cacheService.TryGet("SellingCars", out List<Car> cars))
                _cacheService.Remove("SellingCars");
            if (_cacheService.TryGet("MonthlyReport", out List<ReportData> reports))
                _cacheService.Remove("MonthlyReport");
            return true;
        }


        public async Task DeleteCar(string IdentityNum, string VinCode)
        {
            if(!InputValidator.IsValidIdentityNumber(IdentityNum) && !InputValidator.IsValidVinCode(VinCode))
            {
                throw new InvalidInputException();
            }
            var client = await _clientRepository.Get(x => x.IdentityNumber == IdentityNum && x.DeletedAt == null);
            var car = await _carRepository.Get(x => x.VinCode == VinCode && x.DeletedAt == null);
            if(car.ClientId != client.Id)
            {
                throw new CarDoesNotExistsException();
            }
            await _carRepository.Delete(car);
            if (_cacheService.TryGet("SellingCars", out List<Car> cars))
                _cacheService.Remove("SellingCars");
            if (_cacheService.TryGet("MonthlyReport", out List<ReportData> reports))
                _cacheService.Remove("MonthlyReport");
        }

        public async Task<List<Car>> SellingCarsList(DateInput date)
        {
            if(! _cacheService.TryGet("SellingCars", out List<Car> cars))
            {
                if (date.To < date.From)
                {
                    throw new InvalidInputException();
                }
                cars = await _carRepository.GetByCondition(x => x.DeletedAt == null && x.IsSold == false &&
                             x.StartedSale >= date.From && x.FinishedSale <= date.To);
                if (cars == null || cars.Count == 0)
                {
                    throw new CarDoesNotExistsException();
                }

                _cacheService.Set("SellingCars", cars);
            }
            
            return cars;
        }

        public async Task<List<ReportData>> MonthlyReport()
        {
            if(! _cacheService.TryGet("MonthlyReport", out List<ReportData> reports))
            {
                var carGroups = await _carRepository.GetByCondition(x => x.DeletedAt == null && x.IsSold == true);

                var filteredCars = carGroups.GroupBy(x => x.FinishedSale.Month).Select(x => new { month = x.Key, Cars = x.ToList() }).ToList();

                if (filteredCars == null)
                {
                    throw new CarDoesNotExistsException();
                }

                reports = new List<ReportData>();

                foreach (var car in filteredCars)
                {
                    var report = new ReportData
                    {
                        Month = car.month,
                        CarsAmount = car.Cars.Count
                    };
                    foreach (var item in car.Cars)
                    {
                        report.CarsPrice += item.Price;
                    }
                    report.AveragePrice = (double)report.CarsPrice / report.CarsAmount;
                    reports.Add(report);
                }

                _cacheService.Set("MonthlyReport", reports);
            }
            

            return reports;
        }
    }

}
