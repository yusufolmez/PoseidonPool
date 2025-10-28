using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        private readonly IProductService _productService;

        public GetAllProductsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync();
            return new GetAllProductsQueryResponse
            {
                Products = products
            };
        }
    }
}
