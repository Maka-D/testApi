using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
using CarSales.Repository.RepositoryPattern;
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
        private readonly IRepository<Car> _repository;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IClientService _client;

        public CarService(IRepository<Car> repository, IMapper mapper, AppDbContext appDbContext, IClientService client)
        {
            _repository = repository;
            _appDbContext = appDbContext;
            _mapper = mapper;
            _client = client;
        }
        public async Task<Car> AddCar(string IdentityNumber, CarInput car)
        {
            var client = await _client.GetClient(IdentityNumber);
            if(await CarExists(car.VinCode) )
            {
                throw new CarAlreadyExistsException();
            }
            var insertedCar = _mapper.Map<Car>(car);
            insertedCar.ClientId = client.Id;
            return await _repository.Insert(insertedCar);
        }

        public async Task DeleteCar(string IdentityNum, string VinCode)
        {
            if(string.IsNullOrEmpty(IdentityNum) || string.IsNullOrEmpty(VinCode))
            {
                throw new ArgumentNullException();
            }
            if (! await CarExists(VinCode))
            {
                throw new CarDoesNotExistsException();
            }
            var client = await _client.GetClient(IdentityNum);
            var car = await GetCar(VinCode);
            if(car.ClientId != client.Id )
            {
                throw new CouldNotMatchException("There's no client registered with this car VIN code!");
            }
             _repository.Delete(car);
        }

        public async Task<Car> GetCar(string VinCode)
        {
            if (string.IsNullOrEmpty(VinCode))
            {
                throw new ArgumentNullException("VIN code is Required!");
            }
            var car = await _appDbContext.Cars.Where(x => x.VinCode == VinCode && x.DeletedAt == null).FirstOrDefaultAsync();
            if (car == null)
            {
                throw new CarDoesNotExistsException();
            }
            return car;
        }

        #region private methods
        private async Task<bool> CarExists(string VinCode)
        {
            return await _appDbContext.Cars.Where(x => x.VinCode == VinCode && x.DeletedAt == null).AnyAsync();
        }
        #endregion
    }

}
