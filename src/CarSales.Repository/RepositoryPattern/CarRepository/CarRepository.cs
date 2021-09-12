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

        public CarRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Car>> CarsToSale(DateTime fromDate, DateTime toDate)
        {
            var cars = await (from car in _appDbContext.Cars
                        where car.DeletedAt == null && car.IsSold == false
                        && car.StartedSale >= fromDate && car.FinishedSale <= toDate
                        select car).Include("Client").ToListAsync();
            if(cars == null)
            {
                throw new DoesNotExistsException();
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
                throw new DoesNotExistsException();
            }
            _appDbContext.Cars.Remove(car);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Car> GetCar(string VinCode)
        {
            if (string.IsNullOrEmpty(VinCode))
            {
                throw new ArgumentNullException();
            }
            var car = await _appDbContext.Cars
                .Where(x => x.VinCode == VinCode && x.DeletedAt == null).FirstOrDefaultAsync();

            if(car == null)
            {
                throw new DoesNotExistsException();
            }
            return car;
        }

        public async Task<List<ReportData>> GetCarsByMonth()
        {
            var carGroups = await (from car in _appDbContext.Cars
                       where car.DeletedAt == null && car.IsSold == true
                       group car by DateTime.Parse(car.FinishedSale.ToString()).Month into MonthlySoldCars                      
                       select new  { month = MonthlySoldCars.Key, Cars = MonthlySoldCars.ToList() }).ToListAsync();

            if(carGroups == null)
            {
                throw new DoesNotExistsException();
            }

            var reports = new List<ReportData>();

            foreach(var car in carGroups)
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
                throw new AlreadyExistsException();
            }
            else if(car != null && car.DeletedAt != null)
            {
                entity.DeletedAt = null;
                
                _appDbContext.Cars.Update(entity);
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
                throw new DoesNotExistsException();
            }
            _appDbContext.Cars.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
