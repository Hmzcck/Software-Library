using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Back_End.Services.impl;
public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redisConnection;

    public CacheService(IConnectionMultiplexer redisConnection, IDistributedCache cache)
    {
        _redisConnection = redisConnection;
        _cache = cache;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);

        if (data == null)
        {
            return default(T);
        }

        return JsonConvert.DeserializeObject<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task ClearAsync(string keyPrefix)
    {
        var endPoint = _redisConnection.GetEndPoints().First();
        var server = _redisConnection.GetServer(endPoint);

        var keys = server.Keys(pattern: $"{keyPrefix}*");
        var db = _redisConnection.GetDatabase();

        foreach (var key in keys)
        {
            await db.KeyDeleteAsync(key);
        }
    }

}