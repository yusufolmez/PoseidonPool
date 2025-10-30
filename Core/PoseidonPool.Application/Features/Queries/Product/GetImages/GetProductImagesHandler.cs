using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.GetImages
{
    public class GetProductImagesHandler : IRequestHandler<GetProductImagesQueryRequest, GetProductImagesQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductImagesHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductImagesQueryResponse> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _productService.GetImagesAsync(request.ProductId);
            return new GetProductImagesQueryResponse
            {
                Images = list.Select(x => new ImageItem { Slot = x.slot, Url = x.url }).ToList()
            };
        }
    }
}


