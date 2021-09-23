using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.CacheService
{
    public interface ICacheService
    {
        Task<string> Get(string cacheKey);
        Task Set<T>(string cacheKey, T value);
        Task Remove(string cacheKey);
    }
}
