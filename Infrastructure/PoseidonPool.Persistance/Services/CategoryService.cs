using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.ViewModels.Catalog;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Persistance.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryWriteRepository _categoryWriteRepository;

        public CategoryService(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
        }

        public async Task<CategoryDTO> CreateAsync(VM_CreateCategory model)
        {
            if (string.IsNullOrWhiteSpace(model.CategoryName))
                throw new ArgumentException("CategoryName is required", nameof(model.CategoryName));

            var entity = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = model.CategoryName,
                ImageUrl = model.ImageUrl,
                CreatedDate = DateTime.UtcNow
            };

            await _categoryWriteRepository.AddAsync(entity);
            await _categoryWriteRepository.SaveAsync();

            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var removed = await _categoryWriteRepository.RemoveAsync(id);
            if (!removed) return false;
            await _categoryWriteRepository.SaveAsync();
            return true;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var items = await _categoryReadRepository.GetAll(false).ToListAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<CategoryDTO> GetByIdAsync(string id)
        {
            var item = await _categoryReadRepository.GetByIdAsync(id, false);
            if (item == null) return null;
            return MapToDto(item);
        }

        public async Task<CategoryDTO> UpdateAsync(string id, VM_UpdateCategory model)
        {
            var item = await _categoryReadRepository.GetByIdAsync(id);
            if (item == null) return null;

            if (!string.IsNullOrWhiteSpace(model.CategoryName)) item.CategoryName = model.CategoryName;
            if (!string.IsNullOrWhiteSpace(model.ImageUrl)) item.ImageUrl = model.ImageUrl;
            item.UpdatedDate = DateTime.UtcNow;

            _categoryWriteRepository.Update(item);
            await _categoryWriteRepository.SaveAsync();

            return MapToDto(item);
        }

        private static CategoryDTO MapToDto(Category c)
        {
            return new CategoryDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                ImageUrl = c.ImageUrl,
                ParentId = c.ParentId,
                CreatedDate = c.CreatedDate,
                UpdatedDate = c.UpdatedDate
            };
        }

        public async Task<List<CategoryTreeDTO>> GetTreeAsync()
        {
            var list = await _categoryReadRepository.GetAll(false).ToListAsync();
            var dict = list.ToDictionary(c => c.Id, c => new CategoryTreeDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                ImageUrl = c.ImageUrl,
                ParentId = c.ParentId,
                Children = new List<CategoryTreeDTO>()
            });

            var roots = new List<CategoryTreeDTO>();
            foreach (var c in list)
            {
                if (c.ParentId.HasValue && dict.ContainsKey(c.ParentId.Value))
                {
                    dict[c.ParentId.Value].Children.Add(dict[c.Id]);
                }
                else
                {
                    roots.Add(dict[c.Id]);
                }
            }
            return roots;
        }
    }
}
