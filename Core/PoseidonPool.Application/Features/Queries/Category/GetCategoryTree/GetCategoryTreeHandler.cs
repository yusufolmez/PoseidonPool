using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Category.GetCategoryTree
{
    public class GetCategoryTreeHandler : IRequestHandler<GetCategoryTreeQueryRequest, GetCategoryTreeQueryResponse>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryTreeHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<GetCategoryTreeQueryResponse> Handle(GetCategoryTreeQueryRequest request, CancellationToken cancellationToken)
        {
            var tree = await _categoryService.GetTreeAsync();
            return new GetCategoryTreeQueryResponse { Categories = tree };
        }
    }
}


