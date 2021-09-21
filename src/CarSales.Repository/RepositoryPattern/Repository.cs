using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository.MemoryCacheService;
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
        private readonly DbSet<T> _entities;
        private readonly ICacheService _cache;
        //private string cacheKey = $"{typeof(T)}";

        public Repository(AppDbContext appDbContext)
            //, ICacheService cache)
        {
            _appDbContext = appDbContext;
            _entities = _appDbContext.Set<T>();
            //_cache = cache;
        }

        public async Task<T> Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Add(entity);

            await _appDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
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
            //if(_cache.TryGet($"{typeof(T)}" + entity.Id.ToString(), out entity))
            //    _cache.Remove($"{typeof(T)}" + entity.Id.ToString());

        }

        public async Task<T> Get(Func<T, bool> predicate)
        {

            var entity = await Task.FromResult(_entities.Where(predicate).FirstOrDefault());
            if (entity == null)
            {
                ThrowEntityDoesNotExistsExceptions(entity);
            }

            return entity;

        }

        public async Task<List<T>> GetByCondition(Func<T, bool> predicate)
        {
            //if(! _cache.TryGet(predicate.ToString(),out IEnumerable<T> entities))
            //{
            IEnumerable<T> entities;
                if (typeof(T) == typeof(Car))
                {
                    entities = (IEnumerable<T>)_appDbContext.Cars.Include("Client").Where((Func<Car, bool>)predicate).ToList();                 
                }
                else
                {
                    entities = _entities.Where(predicate).ToList();
                }
            //    _cache.Set(predicate.ToString(), entities);
            //}           
            return   await Task.FromResult(entities.ToList());
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Update(entity);
            await _appDbContext.SaveChangesAsync();
            //_cache.Remove($"{typeof(T)}" + entity.Id.ToString());
            //_cache.Set($"{typeof(T)}" + entity.Id.ToString(), entity);
            return entity;
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
