using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.CacheService
{
    public interface ICacheService
    {
        Task<T> Get<T>(string cacheKey);
        Task Set<T>(string cacheKey, T value, DistributedCacheEntryOptions cacheEntryOptions);
        Task Remove(string cacheKey);
    }
}
