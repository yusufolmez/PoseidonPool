using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.GetComments
{
    public class GetProductCommentsHandler : IRequestHandler<GetProductCommentsQueryRequest, GetProductCommentsQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductCommentsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductCommentsQueryResponse> Handle(GetProductCommentsQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _productService.GetCommentsAsync(request.ProductId);
            return new GetProductCommentsQueryResponse { Comments = list };
        }
    }
}


