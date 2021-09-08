using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RepositoryPattern
{
    public interface IRepository<T> where T :BaseEntity
    {
        Task<bool> Insert(T entity);
        Task<bool> Exists(int Id);
        Task<bool> Update(T entity);
        void Delete(T entity);
        T Get(int Id);
    }
}
