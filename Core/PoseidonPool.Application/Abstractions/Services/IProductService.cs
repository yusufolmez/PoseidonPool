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
        Task<List<ProductDTO>> SearchAsync(string query);
        Task<(List<ProductDTO> items, int totalCount)> GetAllPagedAsync(int page, int pageSize, string sort);
        Task<List<ProductDTO>> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice);
        Task<List<(string slot, string url)>> GetImagesAsync(string productId);
        Task<List<(string slot, string url)>> AddImagesAsync(string productId, Microsoft.AspNetCore.Http.IFormFileCollection files);
        Task<bool> DeleteImageAsync(string productId, string slotOrKey);
        Task<List<DTOs.Comment.UserCommentDTO>> GetCommentsAsync(string productId);
        Task<DTOs.Comment.UserCommentDTO> AddCommentAsync(string productId, ViewModels.Comment.VM_CreateComment model);
        // Accept optional files for upload when creating/updating a product.
        Task<ProductDTO> CreateAsync(VM_CreateProduct model, Microsoft.AspNetCore.Http.IFormFileCollection files = null);
        Task<ProductDTO> UpdateAsync(string id, VM_UpdateProduct model, Microsoft.AspNetCore.Http.IFormFileCollection files = null);
        Task<bool> DeleteAsync(string id);
    }
}
