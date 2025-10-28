using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByIdAsync(string id);
        Task<CategoryDTO> CreateAsync(VM_CreateCategory model);
        Task<CategoryDTO> UpdateAsync(string id, VM_UpdateCategory model);
        Task<bool> DeleteAsync(string id);
    }
}
