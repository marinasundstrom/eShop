namespace YourBrand.StoreFront.Application.Services;

public interface IStoreHandleToStoreIdResolver
{
    Task<string> ToStoreId(string handle, CancellationToken cancellationToken = default);
}


public class StoreHandleToStoreIdResolver : IStoreHandleToStoreIdResolver
{
    private readonly IStoresProvider _storesProvider;

    public StoreHandleToStoreIdResolver(IStoresProvider storesProvider)
    {
        _storesProvider = storesProvider;
    }

    public async Task<string> ToStoreId(string handle, CancellationToken cancellationToken = default)
    {
        var stores = await _storesProvider.GetStores(cancellationToken);

        return stores.First(x => x.Handle == handle).Id;
    }
}
