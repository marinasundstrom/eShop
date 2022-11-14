

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheExtensions
{
    private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
    };

    public async static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
    {
        var result = JsonSerializer.Serialize(value, jsonSerializerOptions);
        await distributedCache.SetStringAsync(key, result, options, cancellationToken);
    }

    public async static Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken cancellationToken = default)
    {
        var result = await distributedCache.GetStringAsync(key, cancellationToken);
        if (result is not null)
        {
            return JsonSerializer.Deserialize<T>(result, jsonSerializerOptions)!;
        }
        return default(T)!;
    }

    public async static Task<T> GetOrCreateAsync<T>(this IDistributedCache distributedCache, string key, Func<DistributedCacheEntryOptions, CancellationToken, Task<T>> factory, CancellationToken cancellationToken = default)
    {
        var result = await distributedCache.GetAsync<T>(key, cancellationToken);
        if (object.Equals(result, default(T)))
        {
            DistributedCacheEntryOptions options = new();

            result = await factory(options, cancellationToken);

            await distributedCache.SetAsync(key, result, options, cancellationToken);
        }
        return result;
    }
}