namespace YourBrand.Catalog;

public sealed class StoreProvider : IStoreProvider
{
    IStoresClient _storesClient;
    IEnumerable<StoreDto> _stores;

    public StoreProvider(IStoresClient storesClient) 
    {
        _storesClient = storesClient;
    }

    public async Task<IEnumerable<StoreDto>> GetAvailableStoresAsync() 
    {
        var items = _stores = (await _storesClient.GetStoresAsync(null, null, null, null, null)).Items;
        await SetCurrentStore(items.First().Id);
        return items;
    }

    public StoreDto? CurrentStore { get; set; }

    public async Task SetCurrentStore(string storeId)
    {
        if(_stores is null)  await GetAvailableStoresAsync();

        CurrentStore = _stores!.FirstOrDefault(x => x.Id == storeId);

        CurrentStoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CurrentStoreChanged;
}

public interface IStoreProvider
{
     Task<IEnumerable<StoreDto>> GetAvailableStoresAsync();

    StoreDto? CurrentStore { get; set; }

    Task SetCurrentStore(string storeId);

    event EventHandler? CurrentStoreChanged;
}