using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandResponse
    {
        public bool Success { get; set; }
        public CategoryDTO Category { get; set; }
        public string Message { get; set; }
    }
}
