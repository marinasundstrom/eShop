using Blazored.LocalStorage;

namespace YourBrand.Catalog;

public interface IStoreProvider
{
     Task<IEnumerable<StoreDto>> GetAvailableStoresAsync();

    StoreDto? CurrentStore { get; set; }

    Task SetCurrentStore(string storeId);

    event EventHandler? CurrentStoreChanged;
}

public sealed class StoreProvider : IStoreProvider
{
    IStoresClient _storesClient;
    private readonly ILocalStorageService _localStorageService;
    IEnumerable<StoreDto> _stores;

    public StoreProvider(IStoresClient storesClient, ILocalStorageService localStorageService) 
    {
        _storesClient = storesClient;
        _localStorageService = localStorageService;
    }

    public async Task<IEnumerable<StoreDto>> GetAvailableStoresAsync() 
    {
        var items = _stores = (await _storesClient.GetStoresAsync(0, null, null, null, null)).Items;
        if(CurrentStore is null) 
        {
            var storeId = await _localStorageService.GetItemAsStringAsync("storeId");
            await SetCurrentStore(storeId ?? items.First().Id);
        } 
        return items;
    }

    public StoreDto? CurrentStore { get; set; }

    public async Task SetCurrentStore(string storeId)
    {
        if(_stores is null) 
        {
            await GetAvailableStoresAsync();
        }

        CurrentStore = _stores!.FirstOrDefault(x => x.Id == storeId);
        
        await _localStorageService.SetItemAsStringAsync("storeId", storeId);

        CurrentStoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CurrentStoreChanged;
}