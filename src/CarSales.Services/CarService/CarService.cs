using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using CarSales.Repository;
using CarSales.Repository.RepositoryPattern;
using CarSales.Repository.RepositoryPattern.CarRepository;
using CarSales.Repository.RepositoryPattern.ClientRepository;
using CarSales.Services.ClientService;
using CarSales.Services.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.CarService
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IMapper mapper, IClientRepository clientRepository)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
        }
        public async Task<Car> AddCar(string IdentityNumber, CarInput car)
        {
            var client = await _clientRepository.GetClient(IdentityNumber);

            var insertedCar = _mapper.Map<Car>(car);
            insertedCar.ClientId = client.Id;
            insertedCar.Client = client;

            return await _carRepository.InsertCar(insertedCar);
        }

        public async Task<bool> BuyCar(string IdentityNum, string VinCode)
        {
            if (string.IsNullOrEmpty(VinCode))
            {
                throw new ArgumentNullException();
            }
            var client = _clientRepository.GetClient(IdentityNum);
            var car = await _carRepository.GetCar(VinCode);
            if(client.Id == car.ClientId || car.IsSold == true)
            {
                throw new CouldNotBuyCarException();
            }
            car.IsSold = true;
            car.FinishedSale = DateTime.Now;
            await _carRepository.UpdateCar(car);
            return true;
        }


        public async Task DeleteCar(string IdentityNum, string VinCode)
        {
            if(string.IsNullOrEmpty(IdentityNum) || string.IsNullOrEmpty(VinCode))
            {
                throw new ArgumentNullException();
            }
            var client = await _clientRepository.GetClient(IdentityNum);
            var car = await _carRepository.GetCar(VinCode);
            if(car.ClientId != client.Id)
            {
                throw new DoesNotExistsException("No such car is registered on this client!");
            }
            await _carRepository.DeleteCar(car);
        }

        public async Task<IEnumerable<Car>> SellingCarsList(DateTime from, DateTime to)
        {
            var cars = await _carRepository.CarsToSale(from, to);
            return cars;
        }

        public async Task<List<ReportData>> MonthlyReport()
        {
            var reportData = await  _carRepository.GetCarsByMonth();
            if(reportData == null)
            {
                throw new DoesNotExistsException();
            }
            return reportData;
        }
    }

}
