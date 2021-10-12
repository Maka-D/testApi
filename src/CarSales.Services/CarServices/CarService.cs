using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using CarSales.Repository.CustomRepositories;
using CarSales.Services.CacheService;
using CarSales.Services.DTOs;
using CarSales.Services.ValidateInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CarSales.Services.CarServices
{
    public class CarService : ICarService
    {
        private readonly CarRepository _carRepository;
        private readonly ClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public CarService(CarRepository carRepository, IMapper mapper
            ,ClientRepository clientRepository
            , ICacheService cacheService)
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

            //checks for car caches, deletes and restores it 
            await CheckCarCache();

            if (await _carRepository.Get(x => x.VinCode == car.VinCode && x.DeletedAt != null && x.Client.IdentityNumber == IdentityNumber) != null)
            {
                return await _carRepository.Update(insertedCar);
            }          

            return await _carRepository.Insert(insertedCar);
        }

        public async Task<bool> BuyCar(IdentifyingData input)
        {
            var client = await _clientRepository.Get(x => x.IdentityNumber == input.IdentityNumber && x.DeletedAt == null);
            if (client == null)
                throw new ClientDoesNotExistsException();
            var car = await _carRepository.Get(x => x.VinCode == input.VinCode && x.DeletedAt == null);
            if (car == null)
                throw new CarDoesNotExistsException();
            if (client.Id == car.ClientId || car.IsSold == true)
            {
                throw new CouldNotBuyCarException();
            }
            car.IsSold = true;
            car.FinishedSale = DateTime.Now;
            await _carRepository.Update(car);

            //checks for car caches, deletes and restores it 
            await CheckCarCache();

            return true;
        }


        public async Task DeleteCar(IdentifyingData input)
        {
            var client = await _clientRepository.Get(x => x.IdentityNumber == input.IdentityNumber && x.DeletedAt == null);
            var car = await _carRepository.Get(x => x.VinCode == input.VinCode && x.DeletedAt == null);
            if (car.ClientId != client.Id)
            {
                throw new CarDoesNotExistsException();
            }
            await _carRepository.Delete(car);

            //checks for car caches, deletes and restores it 
            await CheckCarCache();
        }

        public async Task<List<Car>> SellingCarsList(DateInput date)
        {

            if (date.To < date.From)
            {
                throw new InvalidInputException();
            }

            //gets list of all selling cars from cache or database
            var cachedSellingCars = await GetAllSellingCars();

            var filteredCache = new List<Car>();

            foreach(var car in cachedSellingCars)
            {
                if (car.StartedSale >= date.From && car.FinishedSale <= date.To)
                    filteredCache.Add(car);
            }

            if (filteredCache == null || filteredCache.Count == 0)
            {
                throw new CarDoesNotExistsException();
            }

            return filteredCache;
        }

        public async Task<List<ReportData>> MonthlyReport()
        {
            var cachedReport = await _cacheService.Get<List<ReportData>>("MonthlyReport");

            if (cachedReport == null || cachedReport.Count == 0)
            {
                cachedReport = new List<ReportData>();

                var carGroups = await _carRepository.GetByCondition(x => x.DeletedAt == null && x.IsSold == true);

                var filteredCars = carGroups.GroupBy(x => x.FinishedSale.Month).Select(x => new { month = x.Key, Cars = x.ToList() }).ToList();

                if (filteredCars == null || filteredCars.Count == 0)
                {
                    throw new CarDoesNotExistsException();
                }

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
                    cachedReport.Add(report);
                }

                await _cacheService.Set("MonthlyReport", cachedReport, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }
            

            return cachedReport.ToList();
        }

        private async Task<List<Car>> GetAllSellingCars()
        {
            var cachedSellingCars = await _cacheService.Get<List<Car>>("SellingCars");


            if (cachedSellingCars == null || cachedSellingCars.Count == 0)
            {
                cachedSellingCars = await _carRepository.GetByCondition(x => x.DeletedAt == null && x.IsSold == false);
                if (cachedSellingCars == null || cachedSellingCars.Count == 0)
                {
                    throw new CarDoesNotExistsException();
                }

                await _cacheService.Set("SellingCars", cachedSellingCars, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                    SlidingExpiration = TimeSpan.FromMinutes(15)
                });
            }

            return cachedSellingCars.ToList();
        }

        private async Task CheckCarCache()
        {
            var cachedSellingCars = await _cacheService.Get<List<Car>>("SellingCars");
            if (cachedSellingCars != null)
            {
                await _cacheService.Remove("SellingCars");
                await _cacheService.Set("SellingCars", await GetAllSellingCars(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                    SlidingExpiration = TimeSpan.FromMinutes(15)
                });
            }
               
            var cachedReport = await _cacheService.Get<List<ReportData>>("MonthlyReport");
            if (cachedReport != null)
            {
                await _cacheService.Remove("MonthlyReport");
                await _cacheService.Set("MonthlyReport", await MonthlyReport(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                    SlidingExpiration = TimeSpan.FromMinutes(15)
                });
            }
        }
    }

}
