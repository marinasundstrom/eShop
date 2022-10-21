using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Domain;

namespace Catalog.Application.Products;

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
            var product = await _context.Products
                               .FirstAsync(x => x.Id == request.ProductId);

            var blobId = $"{product.Id}:{request.FileName}";

            await _blobStorageService.DeleteBlobAsync(blobId);

            await _blobStorageService.UploadBlobAsync(blobId, request.Stream);

            product.Image = blobId;

            await _context.SaveChangesAsync();

            return GetImageUrl(product.Image);
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}