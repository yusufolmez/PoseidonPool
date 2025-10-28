using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Commands.Category.UpdateCategory
{
    public class UpdateCategoryCommandResponse
    {
        public bool Success { get; set; }
        public CategoryDTO Category { get; set; }
        public string Message { get; set; }
    }
}
