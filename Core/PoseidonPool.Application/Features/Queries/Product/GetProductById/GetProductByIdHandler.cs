using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductByIdHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var p = await _productService.GetByIdAsync(request.Id);
            return new GetProductByIdQueryResponse
            {
                Product = p
            };
        }
    }
}
