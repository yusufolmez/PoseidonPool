using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Category.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
    {
        private readonly ICategoryService _categoryService;

        public UpdateCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.UpdateAsync(request.Id, request.Model);
            return new UpdateCategoryCommandResponse
            {
                Success = category != null,
                Category = category,
                Message = category != null ? "Category updated." : "Category not found."
            };
        }
    }
}
