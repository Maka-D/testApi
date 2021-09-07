using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RepositoryPattern
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _entities;

        public Repository(AppDbContext appDbContext){
            _appDbContext = appDbContext;
            _entities = _appDbContext.Set<T>();
        }

        public async Task<bool> Exists(int Id)
        {
            var result = await _entities.FirstOrDefaultAsync(x => Id == x.Id);
            if (result.IsActive)
                return true;
            return false;
        }

        public void Insert(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _appDbContext.SaveChanges();
        }
    }
}
