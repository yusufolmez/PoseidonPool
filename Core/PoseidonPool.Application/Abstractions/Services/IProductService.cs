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
        // Accept optional files for upload when creating/updating a product.
        Task<ProductDTO> CreateAsync(VM_CreateProduct model, Microsoft.AspNetCore.Http.IFormFileCollection files = null);
        Task<ProductDTO> UpdateAsync(string id, VM_UpdateProduct model, Microsoft.AspNetCore.Http.IFormFileCollection files = null);
        Task<bool> DeleteAsync(string id);
    }
}
