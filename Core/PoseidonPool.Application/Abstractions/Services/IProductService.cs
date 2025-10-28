using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(string id);
        Task<List<ProductDTO>> GetByBrandAsync(Guid brandId);
        Task<List<ProductDTO>> GetByCategoryAsync(Guid categoryId);
        Task<ProductDTO> CreateAsync(VM_CreateProduct model);
        Task<ProductDTO> UpdateAsync(string id, VM_UpdateProduct model);
        Task<bool> DeleteAsync(string id);
    }
}
