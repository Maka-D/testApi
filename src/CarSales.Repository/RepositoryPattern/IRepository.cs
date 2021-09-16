using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.RepositoryPattern
{
    public interface IRepository<T> where T:BaseEntity
    {
        Task<T> Insert(T entity);
        Task Delete(T entity);
        Task<T> Get(Func<T, bool> predicate);
        Task<List<T>> GetByCondition(Func<T, bool> predicate);
        Task<T> Update(T entity);
    }
}
