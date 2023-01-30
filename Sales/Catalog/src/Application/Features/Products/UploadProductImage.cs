using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public record UploadProductImage(string ProductId, string FileName, Stream Stream) : IRequest<string?>
{
    public class Handler : IRequestHandler<UploadProductImage, string?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;

        public Handler(IApplicationDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<string?> Handle(UploadProductImage request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                               .FirstAsync(x => x.Id == request.ProductId);

            var blobId = $"{item.Id}:{request.FileName}";

            await _blobStorageService.DeleteBlobAsync(blobId);

            await _blobStorageService.UploadBlobAsync(blobId, request.Stream);

            item.Image = blobId;

            await _context.SaveChangesAsync();

            return GetImageUrl(item.Image);
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}