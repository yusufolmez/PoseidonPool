using MediatR;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.Features.Queries.Brand.GetBrandById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Application.Features.Queries.Category.GetCategoryById
{
    public class GetCategoryByIdQueryhandler : IRequestHandler<GetCategoryByIdQueryRequest, GetCategoryByIdQueryResponse>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryByIdQueryhandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<GetCategoryByIdQueryResponse> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var c = await _categoryService.GetByIdAsync(request.Id);
            return new GetCategoryByIdQueryResponse { Category = c };
        }
    }
}
