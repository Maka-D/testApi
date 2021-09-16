using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using CarSales.Repository;
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

        public CarService(IRepository<Car> carRepository, IMapper mapper
            ,IRepository<Client> clientRepository)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
        }
        public async Task<Car> AddCar(string IdentityNumber, CarInput car)
        {
            if (!InputValidator.IsValidIdentityNumber(IdentityNumber))
            {
                throw new InvalidInputException();
            }
            var client = await _clientRepository.Get(x => x.IdentityNumber == IdentityNumber && x.DeletedAt == null);

            var insertedCar = _mapper.Map<Car>(car);
            insertedCar.ClientId = client.Id;
            insertedCar.Client = client;

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
        }

        public async Task<List<Car>> SellingCarsList(DateInput date)
        {
            if(date.To > date.From)
            {
                throw new InvalidInputException();
            }
            var cars = await _carRepository.GetByCondition(x => x.DeletedAt == null && x.IsSold == false &&
                         x.StartedSale >= date.From && x.FinishedSale <= date.To);
            
            return cars;
        }

        public async Task<List<ReportData>> MonthlyReport()
        {
            var carGroups = await  _carRepository.GetByCondition(x => x.DeletedAt == null && x.IsSold == true);

            var filteredCars = carGroups.GroupBy(x => x.FinishedSale.Month).Select(x => new { month = x.Key, Cars = x.ToList() }).ToList();

            if (filteredCars == null)
            {
                throw new CarDoesNotExistsException();
            }

            var reports = new List<ReportData>();

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

            return reports;
        }
    }

}
