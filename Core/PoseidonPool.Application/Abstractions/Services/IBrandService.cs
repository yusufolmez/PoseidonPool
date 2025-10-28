using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IBrandService
    {
        Task<List<BrandDTO>> GetAllAsync();
        Task<BrandDTO> GetByIdAsync(string id);
        Task<BrandDTO> CreateAsync(VM_CreateBrand model);
        Task<BrandDTO> UpdateAsync(string id, VM_UpdateBrand model);
        Task<bool> DeleteAsync(string id);
    }
}
