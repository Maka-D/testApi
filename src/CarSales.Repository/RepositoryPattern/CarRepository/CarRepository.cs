using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.RepositoryPattern.CarRepository
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public CarRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;

        }

        public async Task<List<Car>> CarsToSale(DateTime fromDate, DateTime toDate)
        {
            var cars = await (from car in _appDbContext.Cars
                        where car.DeletedAt == null && car.IsSold == false
                        && car.StartedSale >= fromDate && car.FinishedSale <= toDate
                        select car).Include("Client").ToListAsync();
            if(cars == null)
            {
                throw new CarDoesNotExistsException();
            }
            return cars;              
                       
        }

        public async Task DeleteCar(Car entity)
        {
           if(entity == null)
           {
                throw new ArgumentNullException("entity");
           }
            var car = await _appDbContext.Cars
                .Where(x => x.VinCode == entity.VinCode && x.DeletedAt == null).FirstOrDefaultAsync();

            if(car == null)
            {
                throw new CarDoesNotExistsException();
            }
            _appDbContext.Cars.Remove(car);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Car> GetCar(string VinCode)
        {
            var car = await _appDbContext.Cars
                .Where(x => x.VinCode == VinCode && x.DeletedAt == null).FirstOrDefaultAsync();

            if(car == null)
            {
                throw new CarDoesNotExistsException();
            }
            return car;
        }

        public async Task<List<ReportData>> GetCarsByMonth()
        {
            var carGroups = await _appDbContext.Cars.Where(x => x.DeletedAt == null && x.IsSold == true).ToListAsync();

            var filteredCars = carGroups.GroupBy(x => x.FinishedSale.Month).Select(x => new { month = x.Key, Cars = x.ToList() }).ToList();

            if (filteredCars == null)
            {
                throw new CarDoesNotExistsException();
            }

            var reports = new List<ReportData>();

            foreach(var car in filteredCars)
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

        public async Task<Car> InsertCar(Car entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException();
            }
            var car = await _appDbContext.Cars
                .Where(x => x.VinCode == entity.VinCode).FirstOrDefaultAsync();

            if(car != null && car.DeletedAt == null)
            {
                throw new CarAlreadyExistsException();
            }
            else if(car != null && car.DeletedAt != null)
            {
                car.DeletedAt = null;
                car.ClientId = entity.ClientId;
                car.Brand = entity.Brand;
                car.CarNumber = entity.CarNumber;
                car.Price = entity.Price;
                car.ReleaseDate = entity.ReleaseDate;
                car.StartedSale = entity.StartedSale;
                car.FinishedSale = entity.FinishedSale;
                _appDbContext.Cars.Update(car);
            }
            else
            {
                await _appDbContext.Cars.AddAsync(entity);
            }          
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateCar(Car entity)
        {
           if(entity == null)
           {
                throw new ArgumentNullException();
           }
            var car = await _appDbContext.Cars
                .Where(x => x.VinCode == entity.VinCode && x.DeletedAt == null).FirstOrDefaultAsync();

            if (car == null )
            {
                throw new CarDoesNotExistsException();
            }
            car.Brand = entity.Brand;
            car.CarNumber = entity.CarNumber;
            car.Price = entity.Price;
            car.ReleaseDate = entity.ReleaseDate;
            car.StartedSale = entity.StartedSale;
            car.FinishedSale = entity.FinishedSale;
            _appDbContext.Cars.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
