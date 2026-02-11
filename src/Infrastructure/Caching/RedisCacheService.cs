using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Core.Interfaces;

namespace Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(data))
                return default;
            
            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
            };
            
            await _cache.SetStringAsync(key, json, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var data = await _cache.GetStringAsync(key);
            return !string.IsNullOrEmpty(data);
        }
    }
}
