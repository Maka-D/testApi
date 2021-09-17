using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.RepositoryPattern
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _appDbContext;
        private  DbSet<T> _entities;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _entities = _appDbContext.Set<T>();
        }

        public async Task<T> Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            if(await _entities.Where(x => x.Id == entity.Id && x.DeletedAt == null).AnyAsync())
            {
                ThrowEntityExistsExceptions(entity);
            }
            else if (await _entities.Where(x => x.Id == entity.Id && x.DeletedAt != null).AnyAsync())
            {
                entity.DeletedAt = null;
                _entities.Update(entity);
            }
            else
            {
                _entities.Add(entity);
            }
                
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity is Client)
            {
                //if user wants to delete client that has cars to sell, first we remove cars and then client
                if (await _appDbContext.Cars.Where(x => x.ClientId == entity.Id && x.DeletedAt == null).AnyAsync())
                {
                    var cars = await _appDbContext.Cars.Where(x => x.ClientId == entity.Id && x.DeletedAt == null).ToListAsync();
                    foreach (var car in cars)
                    {
                        car.DeletedAt = DateTime.Now;
                    }
                }

            }
            _entities.Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<T> Get(Func<T, bool> predicate)
        {
            var entity = await Task.FromResult(_entities.Where(predicate).FirstOrDefault());
            if(entity == null)
            {
                ThrowEntityDoesNotExistsExceptions(entity);
            }
            return entity;
        }

        public async Task<List<T>> GetByCondition(Func<T, bool> predicate)
        {
            if (typeof(T) == typeof(Car))
            {
                var cars = (IEnumerable<T>)_appDbContext.Cars.Include("Client").Where((Func<Car, bool>)predicate).ToList();                
                return await Task.FromResult(cars.ToList());
            }
            return await Task.FromResult(_entities.Where(predicate).ToList());
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (await Get(x => x.Id == entity.Id && x.DeletedAt == null) == null)
            {
                ThrowEntityDoesNotExistsExceptions(entity);
            }
            _entities.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        private static void ThrowEntityExistsExceptions(T entity)
        {
            if (typeof(T) == typeof(Client))
                throw new ClientAlreadyExistsException();
            else
                throw new CarAlreadyExistsException();
        }

        private static void ThrowEntityDoesNotExistsExceptions(T entity)
        {
            if (typeof(T) == typeof(Client))
                throw new ClientDoesNotExistsException();
            else
                throw new CarDoesNotExistsException();
        }
    }
}
