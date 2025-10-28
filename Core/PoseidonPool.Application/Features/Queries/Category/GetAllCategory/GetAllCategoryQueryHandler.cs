using MediatR;
using PoseidonPool.Application.Abstractions.Services;
using System.Runtime.CompilerServices;

namespace PoseidonPool.Application.Features.Queries.Category.GetAllCategory
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQueryRequest, GetAllCategoryQueryResponse>
    {
        private readonly ICategoryService _categoryService;
        public GetAllCategoryQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<GetAllCategoryQueryResponse> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var categories =  await _categoryService.GetAllAsync();
            return new GetAllCategoryQueryResponse { Categories = categories };
        }
    }
}

