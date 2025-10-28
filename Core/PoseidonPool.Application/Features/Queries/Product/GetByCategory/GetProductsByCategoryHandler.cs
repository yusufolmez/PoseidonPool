using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.GetByCategory
{
    public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQueryRequest, GetProductsByCategoryQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductsByCategoryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductsByCategoryQueryResponse> Handle(GetProductsByCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetByCategoryAsync(request.CategoryId);
            return new GetProductsByCategoryQueryResponse { Products = products };
        }
    }
}
