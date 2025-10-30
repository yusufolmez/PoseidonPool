using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Category.GetProducts
{
    public class GetProductsByCategoryIdHandler : IRequestHandler<GetProductsByCategoryIdQueryRequest, GetProductsByCategoryIdQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductsByCategoryIdHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductsByCategoryIdQueryResponse> Handle(GetProductsByCategoryIdQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _productService.GetByCategoryAsync(request.CategoryId);
            return new GetProductsByCategoryIdQueryResponse { Products = list };
        }
    }
}


