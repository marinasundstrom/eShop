using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Variants;

public record UploadItemVariantImage(string ItemId, string VariantId, string FileName, Stream Stream) : IRequest<string?>
{
    public class Handler : IRequestHandler<UploadItemVariantImage, string?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;

        public Handler(IApplicationDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<string?> Handle(UploadItemVariantImage request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .Include(x => x.Variants)
                .FirstAsync(x => x.Id == request.ItemId);

            var variant = item.Variants
                .First(x => x.Id == request.VariantId);

            var blobId = $"{variant.Id}:{request.FileName}";

            await _blobStorageService.DeleteBlobAsync(blobId);

            await _blobStorageService.UploadBlobAsync(blobId, request.Stream);

            variant.Image = blobId;

            await _context.SaveChangesAsync();

            return GetImageUrl(variant.Image);
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}