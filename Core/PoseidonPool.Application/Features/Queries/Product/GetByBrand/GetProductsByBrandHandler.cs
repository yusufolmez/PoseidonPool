using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.GetByBrand
{
    public class GetProductsByBrandHandler : IRequestHandler<GetProductsByBrandQueryRequest, GetProductsByBrandQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductsByBrandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductsByBrandQueryResponse> Handle(GetProductsByBrandQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetByBrandAsync(request.BrandId);
            return new GetProductsByBrandQueryResponse { Products = products };
        }
    }
}
