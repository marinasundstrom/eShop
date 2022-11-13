using Microsoft.Extensions.Caching.Distributed;
using YourBrand.Catalog;

namespace YourBrand.StoreFront.Application.Services;

public class StoreHandleToStoreIdResolver : IStoreHandleToStoreIdResolver
{
    private readonly IDistributedCache cache;
    private readonly IStoresClient storesClient;

    public StoreHandleToStoreIdResolver(IDistributedCache cache, IStoresClient storesClient)
    {
        this.cache = cache;
        this.storesClient = storesClient;
    }

    public async Task<string> ToStoreId(string handle, CancellationToken cancellationToken = default)
    {
        var stores = await GetStores(cancellationToken);

        return stores.First(x => x.Handle == handle).Id;
    }

    private async Task<ICollection<StoreDto>> GetStores(CancellationToken cancellationToken)
    {
        return (await cache.GetOrCreateAsync("store", async options =>
        {
            var result = await storesClient.GetStoresAsync(0, 100, null, null, null, cancellationToken);
            return result.Items;
        }))!;
    }
}