using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.CacheService
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _redisCaching;

        public RedisCacheService(IDistributedCache redisCaching)
        {
            _redisCaching = redisCaching;
        }

        public async Task Remove(string cacheKey)
        {
            await _redisCaching.RemoveAsync(cacheKey);
        }

        public async Task Set<T>(string cacheKey, T value, DistributedCacheEntryOptions cacheEntryOptions)
        {
            await _redisCaching.SetStringAsync(cacheKey, JsonConvert.SerializeObject(value), cacheEntryOptions);
        }

        public async Task<T> Get<T>(string cacheKey)
        {
            var stringCache = await _redisCaching.GetStringAsync(cacheKey);
            if (stringCache != null)
                return JsonConvert.DeserializeObject<T>(stringCache);
            else
                return default;           
              
        }
    }
}
