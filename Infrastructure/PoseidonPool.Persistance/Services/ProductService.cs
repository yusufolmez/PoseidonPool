using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.ViewModels.Catalog;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;

namespace PoseidonPool.Persistance.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductDetailWriteRepository _productDetailWriteRepository;
    private readonly IProductImageWriteRepository _productImageWriteRepository;
    private readonly IProductImageReadRepository _productImageReadRepository;
    private readonly PoseidonPool.Application.Abstractions.Storage.IStorageService _storageService;
    private readonly IConfiguration _configuration;

        public ProductService(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IProductDetailWriteRepository productDetailWriteRepository,
            IProductImageWriteRepository productImageWriteRepository,
            IProductImageReadRepository productImageReadRepository,
            PoseidonPool.Application.Abstractions.Storage.IStorageService storageService,
            IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _productDetailWriteRepository = productDetailWriteRepository;
            _productImageWriteRepository = productImageWriteRepository;
            _productImageReadRepository = productImageReadRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

    public async Task<ProductDTO> CreateAsync(VM_CreateProduct model, Microsoft.AspNetCore.Http.IFormFileCollection files = null)
        {
            if (string.IsNullOrWhiteSpace(model.ProductName)) throw new ArgumentException("Ürün adı gerekli");
            if (model.ProductPrice < 0) throw new ArgumentException("Ürün fiyatı 0 veya daha büyük olmalıdır", nameof(model.ProductPrice));

            var entity = new Product
            {
                Id = Guid.NewGuid(),
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                ProductImageId = model.ProductImageId ?? Guid.Empty,
                ProductDetailId = Guid.Empty,
                CategoryId = model.CategoryId ?? Guid.Empty,
                BrandId = model.BrandId ?? Guid.Empty,
                CreatedDate = DateTime.UtcNow
            };

            await _productWriteRepository.AddAsync(entity);
            await _productWriteRepository.SaveAsync();

            if (files != null && files.Count > 0)
            {
                var toUpload = files.Take(5).ToList();
                var uploadPath = $"products/{entity.Id}".Trim('/');
                var fc = new Microsoft.AspNetCore.Http.FormFileCollection();
                fc.AddRange(toUpload);
                var uploaded = await _storageService.UploadAsync(uploadPath, fc);

                var productImage = new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = entity.Id,
                    CreatedDate = DateTime.UtcNow
                };

                for (int i = 0; i < uploaded.Count && i < 5; i++)
                {
                        var fileName = uploaded[i].fileName;
                        var path = uploaded[i].pathOrContainerName;
                        var key = string.IsNullOrWhiteSpace(path) ? fileName : $"{path.Trim('/')}/{fileName}";
                        var url = BuildS3Url(key);

                        switch (i)
                        {
                            case 0: productImage.Image1 = url; productImage.Image1Key = key; break;
                            case 1: productImage.Image2 = url; productImage.Image2Key = key; break;
                            case 2: productImage.Image3 = url; productImage.Image3Key = key; break;
                            case 3: productImage.Image4 = url; productImage.Image4Key = key; break;
                            case 4: productImage.Image5 = url; productImage.Image5Key = key; break;
                        }
                }

                await _productImageWriteRepository.AddAsync(productImage);
                await _productImageWriteRepository.SaveAsync();

                entity.ProductImageId = productImage.Id;
                _productWriteRepository.Update(entity);
                await _productWriteRepository.SaveAsync();
            }

            if (model.ProductDetail != null)
            {
                var productDetail = new ProductDetail
                {
                    Id = Guid.NewGuid(),
                    ProductDescription = model.ProductDetail.ProductDescription,
                    ProductInfo = model.ProductDetail.ProductInfo,
                    ProductId = entity.Id
                };

                await _productDetailWriteRepository.AddAsync(productDetail);
                await _productDetailWriteRepository.SaveAsync();

                entity.ProductDetailId = productDetail.Id;
                _productWriteRepository.Update(entity);
                await _productWriteRepository.SaveAsync();
            }

            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null) return false;

            if (product.ProductImageId != Guid.Empty)
            {
                var img = await _productImageReadRepository.GetByIdAsync(product.ProductImageId.ToString(), false);
                if (img != null)
                {
                    var urlKeyPairs = new (string url, string key)[]
                    {
                        (img.Image1, img.Image1Key),
                        (img.Image2, img.Image2Key),
                        (img.Image3, img.Image3Key),
                        (img.Image4, img.Image4Key),
                        (img.Image5, img.Image5Key)
                    };
                    foreach (var pair in urlKeyPairs)
                    {
                        var effectiveKey = pair.key;
                        if (string.IsNullOrWhiteSpace(effectiveKey) && !string.IsNullOrWhiteSpace(pair.url))
                        {
                            var (_, parsed) = ParseS3Url(pair.url);
                            effectiveKey = parsed;
                        }
                        if (!string.IsNullOrWhiteSpace(effectiveKey))
                        {
                            var (prefix, fileName) = SplitKey(effectiveKey);
                            try
                            {
                                await _storageService.DeleteAsync(prefix, fileName);
                            }
                            catch
                            {
                                // log
                            }
                        }
                    }

                    await _productImageWriteRepository.RemoveAsync(img.Id.ToString());
                    await _productImageWriteRepository.SaveAsync();
                }
            }

            var removed = await _productWriteRepository.RemoveAsync(id);
            if (!removed) return false;
            await _productWriteRepository.SaveAsync();
            return true;
        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            var products = await _productReadRepository.GetAll(false).ToListAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductDTO> GetByIdAsync(string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id, false);
            if (product == null) return null;
            return MapToDto(product);
        }

        public async Task<List<ProductDTO>> GetByBrandAsync(Guid brandId)
        {
            var products = await _productReadRepository.GetWhere(p => p.BrandId == brandId, false).ToListAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<List<ProductDTO>> GetByCategoryAsync(Guid categoryId)
        {
            var products = await _productReadRepository.GetWhere(p => p.CategoryId == categoryId, false).ToListAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductDTO> UpdateAsync(string id, VM_UpdateProduct model, Microsoft.AspNetCore.Http.IFormFileCollection files = null)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null) return null;

            if (model.RemoveImageColumns != null && model.RemoveImageColumns.Length > 0 && product.ProductImageId != Guid.Empty)
            {
                var img = await _productImageReadRepository.GetByIdAsync(product.ProductImageId.ToString(), false);
                if (img != null)
                {
                    var toUpdate = false;
                    foreach (var col in model.RemoveImageColumns)
                    {
                        if (string.IsNullOrWhiteSpace(col)) continue;
                        var colNormalized = col.Trim().ToLowerInvariant();
                        string url = colNormalized switch
                        {
                            "image1" => img.Image1,
                            "image2" => img.Image2,
                            "image3" => img.Image3,
                            "image4" => img.Image4,
                            "image5" => img.Image5,
                            _ => null
                        };
                        string keyFromDb = colNormalized switch
                        {
                            "image1" => img.Image1Key,
                            "image2" => img.Image2Key,
                            "image3" => img.Image3Key,
                            "image4" => img.Image4Key,
                            "image5" => img.Image5Key,
                            _ => null
                        };

                        if (string.IsNullOrWhiteSpace(url) && string.IsNullOrWhiteSpace(keyFromDb)) continue;

                        var keyToDelete = keyFromDb;
                        if (string.IsNullOrWhiteSpace(keyToDelete))
                        {
                            var (_, parsedKey) = ParseS3Url(url);
                            keyToDelete = parsedKey;
                        }
                        if (!string.IsNullOrWhiteSpace(keyToDelete))
                        {
                            var (prefix, fileName) = SplitKey(keyToDelete);
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(fileName))
                                    await _storageService.DeleteAsync(prefix, fileName);
                            }
                            catch
                            {
                                // storage delete hatasını yutuyoruz; DB güncellemesi devam eder
                            }
                        }

                        switch (colNormalized)
                        {
                            case "image1": img.Image1 = null; img.Image1Key = null; break;
                            case "image2": img.Image2 = null; img.Image2Key = null; break;
                            case "image3": img.Image3 = null; img.Image3Key = null; break;
                            case "image4": img.Image4 = null; img.Image4Key = null; break;
                            case "image5": img.Image5 = null; img.Image5Key = null; break;
                        }

                        toUpdate = true;
                    }

                    if (toUpdate)
                    {
                        _productImageWriteRepository.Update(img);
                        await _productImageWriteRepository.SaveAsync();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(model.ProductName))
                product.ProductName = model.ProductName;
            if (model.ProductPrice.HasValue)
                product.ProductPrice = model.ProductPrice.Value;
            if (model.ProductImageId.HasValue)
                product.ProductImageId = model.ProductImageId.Value;
            if (model.ProductDetailId.HasValue)
                product.ProductDetailId = model.ProductDetailId.Value;
            if (model.CategoryId.HasValue)
                product.CategoryId = model.CategoryId.Value;
            if (model.BrandId.HasValue)
                product.BrandId = model.BrandId.Value;

            product.UpdatedDate = DateTime.UtcNow;

            _productWriteRepository.Update(product);
            await _productWriteRepository.SaveAsync();

            if (files != null && files.Count > 0)
            {
                var toUpload = files.Take(5).ToList();
                var uploadPath = $"products/{product.Id}".Trim('/');

                try
                {
                    var existing = _storageService.GetFiles(uploadPath);
                    var hasAnyObject = existing != null && existing.Count > 0;
                    if (!hasAnyObject)
                    {
                        return MapToDto(product);
                    }
                }
                catch
                {
                    return MapToDto(product);
                }

                var fc = new Microsoft.AspNetCore.Http.FormFileCollection();
                fc.AddRange(toUpload);
                var uploaded = await _storageService.UploadAsync(uploadPath, fc);

                var productImage = product.ProductImageId != Guid.Empty
                    ? await _productImageReadRepository.GetByIdAsync(product.ProductImageId.ToString(), false)
                    : null;

                var isNewImageEntity = false;
                if (productImage == null)
                {
                    productImage = new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        CreatedDate = DateTime.UtcNow
                    };
                    isNewImageEntity = true;
                }

                foreach (var up in uploaded)
                {
                    var fileName = up.fileName;
                    var path = up.pathOrContainerName;
                    if (string.IsNullOrWhiteSpace(fileName)) continue;

                    var key = string.IsNullOrWhiteSpace(path) ? fileName : $"{path.Trim('/')}/{fileName}";
                    var url = BuildS3Url(key);

                    if (string.IsNullOrWhiteSpace(productImage.Image1)) { productImage.Image1 = url; productImage.Image1Key = key; continue; }
                    if (string.IsNullOrWhiteSpace(productImage.Image2)) { productImage.Image2 = url; productImage.Image2Key = key; continue; }
                    if (string.IsNullOrWhiteSpace(productImage.Image3)) { productImage.Image3 = url; productImage.Image3Key = key; continue; }
                    if (string.IsNullOrWhiteSpace(productImage.Image4)) { productImage.Image4 = url; productImage.Image4Key = key; continue; }
                    if (string.IsNullOrWhiteSpace(productImage.Image5)) { productImage.Image5 = url; productImage.Image5Key = key; continue; }
  
                }

                if (isNewImageEntity)
                {
                    await _productImageWriteRepository.AddAsync(productImage);
                    await _productImageWriteRepository.SaveAsync();

                    product.ProductImageId = productImage.Id;
                    _productWriteRepository.Update(product);
                    await _productWriteRepository.SaveAsync();
                }
                else
                {
                    _productImageWriteRepository.Update(productImage);
                    await _productImageWriteRepository.SaveAsync();
                }
            }

            return MapToDto(product);
        }

        private static ProductDTO MapToDto(Product p)
        {
            return new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                ProductImageId = p.ProductImageId == Guid.Empty ? (Guid?)null : p.ProductImageId,
                ProductDetailId = p.ProductDetailId == Guid.Empty ? (Guid?)null : p.ProductDetailId,
                CategoryId = p.CategoryId == Guid.Empty ? (Guid?)null : p.CategoryId,
                BrandId = p.BrandId == Guid.Empty ? (Guid?)null : p.BrandId,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            };
        }

        private static (string bucket, string key) ParseS3Url(string url)
        {
            try
            {
                var uri = new Uri(url);
                var host = uri.Host;
                var path = uri.AbsolutePath.TrimStart('/');

                var hostParts = host.Split('.');
                if (hostParts.Length > 0 && hostParts[0] != "s3")
                {
                    var bucket = hostParts[0];
                    var key = path;
                    return (bucket, key);
                }

                var segments = path.Split(new[] { '/' }, 2);
                if (segments.Length == 2)
                {
                    return (segments[0], segments[1]);
                }

                return (null, path);
            }
            catch
            {
                return (null, null);
            }
        }

        private static (string prefix, string fileName) SplitKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return (null, null);
            var k = key.TrimStart('/');

            if (k.EndsWith("/"))
            {
                var prefixOnly = k.TrimEnd('/');
                return (prefixOnly, string.Empty);
            }

            var idx = k.LastIndexOf('/');
            if (idx < 0)
                return (string.Empty, k);

            var prefix = k.Substring(0, idx);
            var fileName = k.Substring(idx + 1);
            return (prefix, fileName);
        }

        private string BuildS3Url(string key)
        {
            var bucket = _configuration?["Storage:Amazon:BucketName"] ?? Environment.GetEnvironmentVariable("STORAGE__Amazon__BucketName");
            var region = _configuration?["AWS:Region"] ?? Environment.GetEnvironmentVariable("AWS_REGION");
            var forcePathStyleStr = _configuration?["Storage:Amazon:ForcePathStyle"] ?? Environment.GetEnvironmentVariable("S3_FORCE_PATH_STYLE");
            var forcePathStyle = false;
            if (!string.IsNullOrWhiteSpace(forcePathStyleStr) && bool.TryParse(forcePathStyleStr, out var fps))
                forcePathStyle = fps;

            var escapedKey = Uri.EscapeDataString(key).Replace("%2F", "/");
            var host = string.IsNullOrWhiteSpace(region) || region == "us-east-1" ? "s3.amazonaws.com" : $"s3.{region}.amazonaws.com";

            return forcePathStyle
                ? $"https://{host}/{bucket}/{escapedKey}"
                : $"https://{bucket}.{host}/{escapedKey}";
        }
    }
}
