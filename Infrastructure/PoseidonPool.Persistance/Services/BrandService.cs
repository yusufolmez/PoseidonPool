using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.ViewModels.Catalog;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;

namespace PoseidonPool.Persistance.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandReadRepository _brandReadRepository;
        private readonly IBrandWriteRepository _brandWriteRepository;

        public BrandService(IBrandReadRepository brandReadRepository, IBrandWriteRepository brandWriteRepository)
        {
            _brandReadRepository = brandReadRepository;
            _brandWriteRepository = brandWriteRepository;
        }

        public async Task<BrandDTO> CreateAsync(VM_CreateBrand model)
        {
            if (string.IsNullOrWhiteSpace(model.BrandName))
                throw new ArgumentException("BrandName is required", nameof(model.BrandName));

            var entity = new Brand
            {
                Id = Guid.NewGuid(),
                BrandName = model.BrandName,
                ImageUrl = model.ImageUrl,
                CreatedDate = DateTime.UtcNow
            };

            await _brandWriteRepository.AddAsync(entity);
            await _brandWriteRepository.SaveAsync();

            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var removed = await _brandWriteRepository.RemoveAsync(id);
            if (!removed) return false;
            await _brandWriteRepository.SaveAsync();
            return true;
        }

        public async Task<List<BrandDTO>> GetAllAsync()
        {
            var items = await _brandReadRepository.GetAll(false).ToListAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<BrandDTO> GetByIdAsync(string id)
        {
            var item = await _brandReadRepository.GetByIdAsync(id, false);
            if (item == null) return null;
            return MapToDto(item);
        }

        public async Task<BrandDTO> UpdateAsync(string id, VM_UpdateBrand model)
        {
            var item = await _brandReadRepository.GetByIdAsync(id);
            if (item == null) return null;

            if (!string.IsNullOrWhiteSpace(model.BrandName)) item.BrandName = model.BrandName;
            if (!string.IsNullOrWhiteSpace(model.ImageUrl)) item.ImageUrl = model.ImageUrl;
            item.UpdatedDate = DateTime.UtcNow;

            _brandWriteRepository.Update(item);
            await _brandWriteRepository.SaveAsync();

            return MapToDto(item);
        }

        private static BrandDTO MapToDto(Brand b)
        {
            return new BrandDTO
            {
                Id = b.Id,
                BrandName = b.BrandName,
                ImageUrl = b.ImageUrl,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            };
        }
    }
}
