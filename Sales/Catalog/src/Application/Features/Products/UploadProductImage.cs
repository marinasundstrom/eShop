using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UploadProductImage(long ProductId, string FileName, Stream Stream) : IRequest<Result<string>>
{
    public sealed class Handler : IRequestHandler<UploadProductImage, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;

        public Handler(IApplicationDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<Result<string>> Handle(UploadProductImage request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure<string>(Errors.Products.ProductNotFound);
            }

            var blobId = $"{item.Id}:{request.FileName}";

            await _blobStorageService.DeleteBlobAsync(blobId);

            await _blobStorageService.UploadBlobAsync(blobId, request.Stream);

            item.Image = blobId;

            await _context.SaveChangesAsync();

            return Result.Success(GetImageUrl(item.Image)!);
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}