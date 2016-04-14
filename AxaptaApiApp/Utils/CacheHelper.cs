using System;
using System.Runtime.Caching;

namespace AxaptaApiApp.Utils
{
    public static class MemoryCacheHelper
    {
        public static T GetValue<T>(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;

            return (T)memoryCache.Get(key);
        }

        public static bool Add<T>(string key, T value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;

            return memoryCache.Add(key, value, absExpiration);
        }

        public static void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;

            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }
}