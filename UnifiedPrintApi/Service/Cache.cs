﻿using System.Security.Cryptography;
using System.Text;

namespace UnifiedPrintApi.Service;

public class Cache
{
    public record CacheEntry(object Item, DateTimeOffset? ExpiryTime);
    private Dictionary<string, CacheEntry> _cache = new(); // TODO: Load from file where expiryTime is infinite

    public T? CacheValue<T>(string key, Func<T?> generateValue) =>
        CacheValue(key, generateValue, TimeSpan.FromHours(3));
    public T? CacheValue<T>(string key, Func<T?> generateValue, TimeSpan? expireFromNow)
    {
        if (_cache.ContainsKey(key))
        {
            CacheEntry entry = _cache[key];
            if (entry.ExpiryTime > DateTimeOffset.Now)
            {
                return (T)entry.Item;
            }
        }

        T? value = generateValue();

        if (value == null)
            return default;
        
        CacheEntry cache = new(value, DateTimeOffset.Now + expireFromNow);
        _cache[key] = cache;
        return value;
    }
    
    public static string Hash(string input)
    {
        using var sha1 = SHA1.Create();
        return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }
}