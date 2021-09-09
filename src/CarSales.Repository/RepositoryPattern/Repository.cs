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
    public class Repository<T> : IRepository<T> where T :BaseEntity
    {

        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _entities;

        public Repository(AppDbContext appDbContext){
            _appDbContext = appDbContext;
            _entities = _appDbContext.Set<T>();
        }

        public async Task<bool> Update(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T> Insert(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            else if( _entities.Where(x => x.Id == entity.Id && x.DeletedAt == null).Any())
            {
                throw new ClientAlreadyExistsException();

            }
            else if (_entities.Where(x => x.Id == entity.Id && x.DeletedAt != null).Any())
            {
                var entityData = await _entities.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
                entityData.DeletedAt = null;
                _entities.Update(entity);
                await _appDbContext.SaveChangesAsync();
                return entity;
            }
            await _entities.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async void  Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //if user wants to delete client that has cars to sell, first we remove cars and then client
            else if (typeof(T).Equals(_appDbContext.Clients))
            {
                if(await _appDbContext.Cars.Where(x => x.ClientId == entity.Id && x.DeletedAt == null).AnyAsync())
                {
                    var cars = await _appDbContext.Cars.Where(x => x.ClientId == entity.Id && x.DeletedAt == null).ToListAsync();
                    foreach(var car in cars)
                    {
                        car.DeletedAt = DateTime.Now;
                    }
                    _entities.Remove(entity);
                    await _appDbContext.SaveChangesAsync();
                }

            }
            else
            {
                _entities.Remove(entity);
                await _appDbContext.SaveChangesAsync();
            }
             
        }

        public async Task<T> Get(int Id)
        {
            var entity = await _entities.Where(x => x.Id == Id).FirstOrDefaultAsync();
            return entity;
        }

        //public async Task<List<T>> GetAll()
        //{
        //    if(typeof(T).Equals(_appDbContext.Cars))
        //    {
        //        return await _appDbContext.Cars.Include(x => x.Client).ToListAsync();
        //    }
        //    return await _entities.ToListAsync();
        //}
    }
}
