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
        private readonly DbSet<T> _entities;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _entities = _appDbContext.Set<T>();
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

            _entities.Remove(entity);
            await _appDbContext.SaveChangesAsync();

        }

        public async Task<T> Get(Func<T, bool> predicate)
        {

            return await Task.FromResult(_entities.Where(predicate).FirstOrDefault());

        }

        public async Task<List<T>> GetByCondition(Func<T, bool> predicate)
        {

            var entities = _entities.Where(predicate).ToList();

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
            return entity;
        }

        
    }
}
