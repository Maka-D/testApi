using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RepositoryPattern
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Insert(T entity);
        Task<bool> Exists(int Id);

    }
}
