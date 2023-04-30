using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

using YourBrand.Catalog;

namespace YourBrand.StoreFront.Application.Services;

public interface IStoresProvider
{
    Task<ICollection<StoreDto>> GetStores(CancellationToken cancellationToken);

    Task<StoreDto?> GetStore(string id, CancellationToken cancellationToken);

    Task<StoreDto?> GetCurrentStore(CancellationToken cancellationToken);
}

public sealed class StoresProvider : IStoresProvider
{
    private readonly IDistributedCache cache;
    private readonly IStoresClient storesClient;
    private readonly ICurrentUserService _currentUserService;

    public StoresProvider(IDistributedCache cache, IStoresClient storesClient, ICurrentUserService currentUserService)
    {
        this.cache = cache;
        this.storesClient = storesClient;
        _currentUserService = currentUserService;
    }

    public async Task<ICollection<StoreDto>> GetStores(CancellationToken cancellationToken)
    {
        return (await cache.GetOrCreateAsync("store", async (options, cancellationToken) =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
            
            var result = await storesClient.GetStoresAsync(0, 100, null, null, null, cancellationToken);
            return result.Items;
        }))!;
    }

    public async Task<StoreDto?> GetStore(string id, CancellationToken cancellationToken)
    {
        var stores = await GetStores(cancellationToken);
        return stores.FirstOrDefault(x => x.Id == id);
    }

    public async Task<StoreDto?> GetCurrentStore(CancellationToken cancellationToken)
    {
        var handle = _currentUserService.Host;

        var stores = await GetStores(cancellationToken);
        return stores.FirstOrDefault(x => x.Handle == handle);
    }
}