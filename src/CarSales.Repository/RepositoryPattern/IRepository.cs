using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.RepositoryPattern
{
    public interface IRepository<T> where T :BaseEntity
    {
        Task<T> Insert(T entity);
        Task<bool> Update(T entity);
        void Delete(T entity);
        //Task<T> Get(int Id);
        //Task<List<T>> GetAll();
    }
}
