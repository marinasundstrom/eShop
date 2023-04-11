using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Import;

public sealed record ImportProducts(Stream Stream) : IRequest<IEnumerable<string>>
{
    public sealed class Handler : IRequestHandler<ImportProducts, IEnumerable<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;

        public Handler(IApplicationDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<IEnumerable<string>> Handle(ImportProducts request, CancellationToken cancellationToken)
        {
            var name = DateTime.UtcNow.Ticks.ToString();

            await UploadAndExtractFiles(request.Stream, name);

            using var fileStream = File.OpenRead($"./uploads/{name}/products.csv");

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };
            
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<ProductRecord>();

                foreach (var record in records)
                {
                    Store store = await GetStore(record.StoreIdOrHandle, cancellationToken);

                    string productHandle = GetHandle(record);

                    var p = _context.Products.Any(x => x.SKU == record.Sku || x.Handle == record.Handle);

                    if(p) continue;

                    ProductGroup group = await GetGroup(store, record.GroupId.GetValueOrDefault(), cancellationToken);
                    
                    var parentProduct = string.IsNullOrEmpty(record.ParentSku) ? null : await GetProduct(store, record.ParentSku, cancellationToken);

                    products.Add(record.Sku, new Product(record.Name, productHandle)
                    {
                        SKU = record.Sku,
                        Description = record.Description,
                        Image = record.Image,
                        Price = record.Price,
                        RegularPrice = record.RegularPrice,
                        Group = group,
                        ParentProduct = parentProduct,
                        Store = store,
                        Visibility = record.Listed.GetValueOrDefault() ? Domain.Enums.ProductVisibility.Listed : Domain.Enums.ProductVisibility.Unlisted
                    });

                    group.IncrementProductCount();
                }
            }

            _context.Products.AddRange(products.Select(x => x.Value));

            await _context.SaveChangesAsync(cancellationToken);

            List<string> errors = new ();

            foreach(var product in products.Select(x => x.Value)) 
            {
                if(string.IsNullOrEmpty(product.Image)) continue;

                var image = string.IsNullOrEmpty(product.Image) ? null : Path.Combine("./temp", product.Image!);

                var blobId = $"{product.Id}:{product.Image}";

                await _blobStorageService.DeleteBlobAsync(blobId);

                try 
                {
                    await _blobStorageService.UploadBlobAsync(blobId, File.OpenRead(image!));        
                    product.Image = blobId;

                    File.Delete(image!);    
                }
                catch(Exception exc) 
                {
                    errors.Add($"{exc.Message}");
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            Directory.Delete($"./uploads/{name}", true);

            return errors;
        }

        private static string GetHandle(ProductRecord record)
        {
            return record.Handle ?? record.Name
                .ToLower()
                .Replace("-", string.Empty);
        }

        Dictionary<long, ProductGroup> groups = new Dictionary<long, ProductGroup>();

        private async Task<ProductGroup> GetGroup(Store store, long groupId, CancellationToken cancellationToken)
        {
            if(!groups.TryGetValue(groupId, out var group)) 
            {
                group = await _context.ProductGroups
                    .Where(x => x.Store == store)
                    .Include(x => x.Parent)
                    .FirstAsync(x => x.Id == groupId, cancellationToken);

                groups.Add(groupId, group);
            }
            return group;
        }

        Dictionary<string, Store> stores = new Dictionary<string, Store>();

        private async Task<Store> GetStore(string handle, CancellationToken cancellationToken)
        {
            if(!stores.TryGetValue(handle, out var store)) 
            {
                store = await _context.Stores.FirstAsync(x => x.Handle == handle, cancellationToken);
                stores.Add(handle, store);
            }
            return store;
        }

        Dictionary<string, Product> products = new Dictionary<string, Product>();

        private async Task<Product> GetProduct(Store store, string sku, CancellationToken cancellationToken)
        {
            if(!products.TryGetValue(sku, out var product)) 
            {
                product = await _context.Products
                    .Where(x => x.SKU == sku)
                    .FirstAsync(x => x.SKU == sku, cancellationToken);

                products.Add(sku, product);
            }
            return product;
        }

        async Task UploadAndExtractFiles(Stream stream, string name) 
        {
            try
            {
                Directory.CreateDirectory($"./uploads");
            }
            catch(IOException) {}

            using (var file = File.Open($"./uploads/{name}.zip", FileMode.OpenOrCreate)) 
            {
                await stream.CopyToAsync(file);
                file.Seek(0, SeekOrigin.Begin);
            }

            try
            {
                Directory.CreateDirectory($"./uploads/{name}");
            }
            catch(IOException) {}

            System.IO.Compression.ZipFile.ExtractToDirectory($"./uploads/{name}.zip", $"./uploads/{name}");

            File.Delete($"./uploads/{name}.zip");
        }

        public record class ProductRecord(string StoreIdOrHandle, string Sku, string Name, string? Handle, string? Description, long? GroupId, string? GroupName, string? ParentSku, string? Image, decimal Price, decimal? RegularPrice, bool? Listed);
    }
}