using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.CacheService
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _redisCaching;
        private DistributedCacheEntryOptions _cacheEntryOptions;

        public RedisCacheService(IDistributedCache redisCaching)
        {
            _redisCaching = redisCaching;
            _cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
        }

        public async Task Remove(string cacheKey)
        {
            await _redisCaching.RemoveAsync(cacheKey);
        }

        public async Task Set<T>(string cacheKey, T value)
        {
            var encodedValue = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            await _redisCaching.SetAsync(cacheKey, encodedValue, _cacheEntryOptions);
        }

        public async Task<string> Get(string cacheKey)
        {
            var cacheInBytes = await _redisCaching.GetAsync(cacheKey);
            if (cacheInBytes == null)
                return null;
            return Encoding.UTF8.GetString(cacheInBytes);


        }
    }
}
