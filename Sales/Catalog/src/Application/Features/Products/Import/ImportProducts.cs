using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Import;

public record ProductImportResult(IEnumerable<string> Diagnostics);

public sealed record ImportProducts(Stream Stream) : IRequest<Result<ProductImportResult>>
{

    public sealed class Handler : IRequestHandler<ImportProducts, Result<ProductImportResult>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;

        public Handler(IApplicationDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<Result<ProductImportResult>> Handle(ImportProducts request, CancellationToken cancellationToken)
        {
            var name = DateTime.UtcNow.Ticks.ToString();

            await UploadAndExtractFiles(request.Stream, name);

            using var fileStream = File.OpenRead($"./uploads/{name}/products.csv");

            List<string> diagnostics = new ();

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

                    var productExists = _context.Products
                        .Where(x => x.Store == store)
                        .Any(x => x.SKU == record.Sku || x.Handle == record.Handle);

                    if(productExists) 
                    {
                        diagnostics.Add($"Product with SKU \"{record.Sku}\" already exists. Skipping it.");
                        continue;
                    }

                    Brand? brand = string.IsNullOrEmpty(record.Brand) ? null : await GetBrand(record.Brand, cancellationToken);

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

            foreach(var product in products.Select(x => x.Value)) 
            {
                if(string.IsNullOrEmpty(product.Image))
                {
                     continue;
                }

                var image = string.IsNullOrEmpty(product.Image) ? null : Path.Combine("./temp", product.Image!);

                var blobId = $"{product.Id}:{product.Image}";

                await _blobStorageService.DeleteBlobAsync(blobId);

                Stream? stream = null;

                try 
                {
                    stream  = File.OpenRead(image!);
                } 
                catch (FileNotFoundException) 
                {
                    diagnostics.Add($"Image \"{image}\" not found for SKU \"{product.SKU}\".");

                    continue;
                }

                try 
                {
                    await _blobStorageService.UploadBlobAsync(blobId, stream!);        
                    product.Image = blobId;

                    File.Delete(image!);    
                }
                catch(Exception exc) 
                {
                    diagnostics.Add($"{exc.Message}");
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            string ArchiveDirPath = $"./uploads/{name}";

            Directory.Delete(ArchiveDirPath, true);

            return Result.Success(new ProductImportResult(diagnostics));
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

        Dictionary<string, Brand> brands = new Dictionary<string, Brand>();

        private async Task<Brand> GetBrand(string handle, CancellationToken cancellationToken)
        {
            if(!brands.TryGetValue(handle, out var brand)) 
            {
                brand = await _context.Brands.FirstAsync(x => x.Handle == handle, cancellationToken);
                brands.Add(handle, brand);
            }
            return brand;
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
            const string UploadDirPath = $"./uploads";

            try
            {
                Directory.CreateDirectory(UploadDirPath);
            }
            catch(IOException) {}

            string ArchiveFilePath = $"./uploads/{name}.zip";

            using (var file = File.Open(ArchiveFilePath, FileMode.OpenOrCreate)) 
            {
                await stream.CopyToAsync(file);
                file.Seek(0, SeekOrigin.Begin);
            }

            string ArchiveDirPath = $"./uploads/{name}";

            try
            {
                Directory.CreateDirectory(ArchiveDirPath);
            }
            catch(IOException) {}

            System.IO.Compression.ZipFile.ExtractToDirectory(ArchiveFilePath, ArchiveDirPath);

            File.Delete(ArchiveFilePath);
        }

        public record class ProductRecord(string StoreIdOrHandle, string Sku, string Name, string? Handle, string? Description, string? Brand, long? GroupId, string? GroupName, string? ParentSku, string? Image, decimal Price, decimal? RegularPrice, bool? Listed);
    }
}