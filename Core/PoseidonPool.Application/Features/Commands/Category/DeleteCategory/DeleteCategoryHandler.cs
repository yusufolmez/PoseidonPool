using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Category.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
    {
        private readonly ICategoryService _categoryService;

        public DeleteCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var success = await _categoryService.DeleteAsync(request.Id);
            return new DeleteCategoryCommandResponse
            {
                Success = success,
                Message = success ? "Category deleted." : "Category not found."
            };
        }
    }
}
