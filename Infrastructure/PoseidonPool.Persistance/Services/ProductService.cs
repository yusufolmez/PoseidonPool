using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
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

        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IProductDetailWriteRepository productDetailWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _productDetailWriteRepository = productDetailWriteRepository;
        }

        public async Task<ProductDTO> CreateAsync(VM_CreateProduct model)
        {
            if (string.IsNullOrWhiteSpace(model.ProductName)) throw new ArgumentException("Ürün adı gerekli");
            if (model.ProductPrice < 0) throw new ArgumentException("Ürün fiyatı 0 veya daha büyük olmalıdır", nameof(model.ProductPrice));

            // create product first (principal) to avoid FK violation from ProductDetail -> Product
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

            // if client supplied nested detail, create it pointing to saved product
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

                // link back the product to the created detail
                entity.ProductDetailId = productDetail.Id;
                _productWriteRepository.Update(entity);
                await _productWriteRepository.SaveAsync();
            }

            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var removed = await _productWriteRepository.RemoveAsync(id);
            if (!removed)
                return false;
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

        public async Task<ProductDTO> UpdateAsync(string id, VM_UpdateProduct model)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null) return null;

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
    }
}
