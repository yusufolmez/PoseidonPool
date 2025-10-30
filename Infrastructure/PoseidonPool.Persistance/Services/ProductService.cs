using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.ViewModels.Catalog;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Application.Repositories.Comment;
using PoseidonPool.Domain.Entities.Comment;
using PoseidonPool.Application.DTOs.Comment;
using PoseidonPool.Application.ViewModels.Comment;

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
        private readonly IUserCommentReadRepository _userCommentReadRepository;
        private readonly IUserCommentWriteRepository _userCommentWriteRepository;

        public ProductService(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IProductDetailWriteRepository productDetailWriteRepository,
            IProductImageWriteRepository productImageWriteRepository,
            IProductImageReadRepository productImageReadRepository,
            PoseidonPool.Application.Abstractions.Storage.IStorageService storageService,
            IConfiguration configuration,
            IUserCommentReadRepository userCommentReadRepository,
            IUserCommentWriteRepository userCommentWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _productDetailWriteRepository = productDetailWriteRepository;
            _productImageWriteRepository = productImageWriteRepository;
            _productImageReadRepository = productImageReadRepository;
            _storageService = storageService;
            _configuration = configuration;
            _userCommentReadRepository = userCommentReadRepository;
            _userCommentWriteRepository = userCommentWriteRepository;
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
            var dto = MapToDto(product);
            try
            {
                var q = _userCommentReadRepository.GetWhere(c => c.ProductId == product.Id && c.Status == true, false);
                var total = await q.CountAsync();
                if (total > 0)
                {
                    var sum = await q.SumAsync(c => (int?)c.Rating) ?? 0;
                    dto.AverageRating = Math.Round((double)sum / total, 2);
                    dto.TotalComments = total;
                }
                else
                {
                    dto.AverageRating = 0;
                    dto.TotalComments = 0;
                }
            }
            catch { /* ignore rating calc errors */ }
            return dto;
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

        public async Task<List<ProductDTO>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<ProductDTO>();

            var normalized = query.Trim();
            var products = await _productReadRepository
                .GetWhere(p => p.ProductName.Contains(normalized), false)
                .ToListAsync();

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

        public async Task<(List<ProductDTO> items, int totalCount)> GetAllPagedAsync(int page, int pageSize, string sort)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 20;

            var queryable = _productReadRepository.GetAll(false);

            // sorting
            switch ((sort ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "price_asc":
                    queryable = queryable.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    queryable = queryable.OrderByDescending(p => p.ProductPrice);
                    break;
                case "name_asc":
                    queryable = queryable.OrderBy(p => p.ProductName);
                    break;
                case "name_desc":
                    queryable = queryable.OrderByDescending(p => p.ProductName);
                    break;
                case "created_asc":
                    queryable = queryable.OrderBy(p => p.CreatedDate);
                    break;
                case "created_desc":
                default:
                    queryable = queryable.OrderByDescending(p => p.CreatedDate);
                    break;
            }

            var total = await queryable.CountAsync();
            var pageItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pageItems.Select(MapToDto).ToList(), total);
        }

        public async Task<List<ProductDTO>> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice)
        {
            decimal? min = minPrice;
            decimal? max = maxPrice;
            if (min.HasValue && max.HasValue && min.Value > max.Value)
            {
                var tmp = min.Value;
                min = max;
                max = tmp;
            }

            var query = _productReadRepository.GetAll(false);
            if (min.HasValue)
                query = query.Where(p => p.ProductPrice >= min.Value);
            if (max.HasValue)
                query = query.Where(p => p.ProductPrice <= max.Value);

            var list = await query.OrderByDescending(p => p.CreatedDate).ToListAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<List<(string slot, string url)>> GetImagesAsync(string productId)
        {
            var product = await _productReadRepository.GetByIdAsync(productId, false);
            if (product == null) return new List<(string, string)>();

            if (product.ProductImageId == Guid.Empty)
                return new List<(string, string)>();

            var img = await _productImageReadRepository.GetByIdAsync(product.ProductImageId.ToString(), false);
            if (img == null) return new List<(string, string)>();

            var list = new List<(string slot, string url)>();
            if (!string.IsNullOrWhiteSpace(img.Image1)) list.Add(("image1", img.Image1));
            if (!string.IsNullOrWhiteSpace(img.Image2)) list.Add(("image2", img.Image2));
            if (!string.IsNullOrWhiteSpace(img.Image3)) list.Add(("image3", img.Image3));
            if (!string.IsNullOrWhiteSpace(img.Image4)) list.Add(("image4", img.Image4));
            if (!string.IsNullOrWhiteSpace(img.Image5)) list.Add(("image5", img.Image5));
            return list;
        }

        public async Task<List<(string slot, string url)>> AddImagesAsync(string productId, Microsoft.AspNetCore.Http.IFormFileCollection files)
        {
            var product = await _productReadRepository.GetByIdAsync(productId);
            if (product == null) return new List<(string, string)>();

            var toUpload = files?.Where(f => f != null && f.Length > 0).Take(5).ToList() ?? new List<Microsoft.AspNetCore.Http.IFormFile>();
            if (toUpload.Count == 0) return await GetImagesAsync(productId);

            var uploadPath = $"products/{product.Id}".Trim('/');
            var fc = new Microsoft.AspNetCore.Http.FormFileCollection();
            fc.AddRange(toUpload);
            var uploaded = await _storageService.UploadAsync(uploadPath, fc);

            var productImage = product.ProductImageId != Guid.Empty
                ? await _productImageReadRepository.GetByIdAsync(product.ProductImageId.ToString(), false)
                : null;

            var isNew = false;
            if (productImage == null)
            {
                productImage = new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    CreatedDate = DateTime.UtcNow
                };
                isNew = true;
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

            if (isNew)
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

            return await GetImagesAsync(productId);
        }

        public async Task<bool> DeleteImageAsync(string productId, string slotOrKey)
        {
            if (string.IsNullOrWhiteSpace(slotOrKey)) return false;
            var product = await _productReadRepository.GetByIdAsync(productId);
            if (product == null) return false;
            if (product.ProductImageId == Guid.Empty) return false;

            var img = await _productImageReadRepository.GetByIdAsync(product.ProductImageId.ToString(), false);
            if (img == null) return false;

            string keyToDelete = null;
            string slot = slotOrKey.Trim().ToLowerInvariant();
            switch (slot)
            {
                case "image1": keyToDelete = img.Image1Key; img.Image1 = null; img.Image1Key = null; break;
                case "image2": keyToDelete = img.Image2Key; img.Image2 = null; img.Image2Key = null; break;
                case "image3": keyToDelete = img.Image3Key; img.Image3 = null; img.Image3Key = null; break;
                case "image4": keyToDelete = img.Image4Key; img.Image4 = null; img.Image4Key = null; break;
                case "image5": keyToDelete = img.Image5Key; img.Image5 = null; img.Image5Key = null; break;
                default:
                    // slot adı değilse URL olabilir; URL'den key çıkar
                    var (_, parsedKey) = ParseS3Url(slotOrKey);
                    keyToDelete = parsedKey;
                    // eşleşen slotu da temizle
                    if (!string.IsNullOrWhiteSpace(keyToDelete))
                    {
                        if (img.Image1Key == keyToDelete) { img.Image1 = null; img.Image1Key = null; }
                        else if (img.Image2Key == keyToDelete) { img.Image2 = null; img.Image2Key = null; }
                        else if (img.Image3Key == keyToDelete) { img.Image3 = null; img.Image3Key = null; }
                        else if (img.Image4Key == keyToDelete) { img.Image4 = null; img.Image4Key = null; }
                        else if (img.Image5Key == keyToDelete) { img.Image5 = null; img.Image5Key = null; }
                    }
                    break;
            }

            if (!string.IsNullOrWhiteSpace(keyToDelete))
            {
                var (prefix, fileName) = SplitKey(keyToDelete);
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    try { await _storageService.DeleteAsync(prefix, fileName); } catch { /* log swallow */ }
                }
            }

            _productImageWriteRepository.Update(img);
            await _productImageWriteRepository.SaveAsync();
            return true;
        }

        public async Task<List<UserCommentDTO>> GetCommentsAsync(string productId)
        {
            var list = await _userCommentReadRepository
                .GetWhere(c => c.ProductId.ToString() == productId, false)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return list.Select(MapCommentToDto).ToList();
        }

        public async Task<UserCommentDTO> AddCommentAsync(string productId, VM_CreateComment model)
        {
            if (string.IsNullOrWhiteSpace(productId)) throw new ArgumentException("productId gerekli", nameof(productId));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(model.CommentDetail)) throw new ArgumentException("Yorum metni gerekli", nameof(model.CommentDetail));

            var prod = await _productReadRepository.GetByIdAsync(productId);
            if (prod == null) throw new InvalidOperationException("Ürün bulunamadı");

            var entity = new UserComment
            {
                Id = Guid.NewGuid(),
                CustomerId = model.CustomerId,
                ImageUrl = model.ImageUrl,
                CommentDetail = model.CommentDetail,
                Rating = model.Rating,
                Status = model.Status,
                ProductId = prod.Id,
                CreatedDate = DateTime.UtcNow
            };

            await _userCommentWriteRepository.AddAsync(entity);
            await _userCommentWriteRepository.SaveAsync();

            return MapCommentToDto(entity);
        }

        private static UserCommentDTO MapCommentToDto(UserComment c)
        {
            return new UserCommentDTO
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                ImageUrl = c.ImageUrl,
                CommentDetail = c.CommentDetail,
                Rating = c.Rating,
                Status = c.Status,
                ProductId = c.ProductId,
                CreatedDate = c.CreatedDate
            };
        }
    }
}
