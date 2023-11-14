using Microsoft.Extensions.Caching.Memory;

namespace Presentation.Services.Infra.Cache
{
    public class CacheStorage : ICacheStorage
    {
        private readonly IMemoryCache cache;

        public CacheStorage(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void Insert(string key, object value)
        {
            cache.Set(key, value);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }
    }
}