namespace YourBrand.StoreFront.Application.Services;

public interface IStoreHandleToStoreIdResolver
{
    Task<string> ToStoreId(string handle, CancellationToken cancellationToken = default);
}
