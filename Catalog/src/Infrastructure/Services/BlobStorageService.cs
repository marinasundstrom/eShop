using Catalog.Application.Services;
using Azure.Storage.Blobs;

namespace Catalog.Infrastructure.Services;

public sealed class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        this._blobServiceClient = blobServiceClient;
    }

    public Task DeleteBlobAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetBlobAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task UploadBlobAsync(string id, Stream stream)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("images");

#if DEBUG
        await blobContainerClient.CreateIfNotExistsAsync();
#endif

        await blobContainerClient.UploadBlobAsync(id, stream);
    }
}