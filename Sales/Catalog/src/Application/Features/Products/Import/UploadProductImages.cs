using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Import;

public sealed record UploadProductImages(Stream Stream) : IRequest
{
    public sealed class Handler : IRequestHandler<UploadProductImages>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;

        public Handler(IApplicationDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<Unit> Handle(UploadProductImages request, CancellationToken cancellationToken)
        {
            using (var file = File.Open("./temp.zip", FileMode.OpenOrCreate)) 
            {
                await request.Stream.CopyToAsync(file);
                file.Seek(0, SeekOrigin.Begin);
            }

            try
            {
                Directory.CreateDirectory("./temp");
            }
            catch(IOException) {}

            System.IO.Compression.ZipFile.ExtractToDirectory("./temp.zip", "./temp");

            //var fils = Directory.GetFiles("./temp");

            return Unit.Value;
        }
    }
}