using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.FilterByPrice
{
    public class FilterByPriceHandler : IRequestHandler<FilterByPriceQueryRequest, FilterByPriceQueryResponse>
    {
        private readonly IProductService _productService;

        public FilterByPriceHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<FilterByPriceQueryResponse> Handle(FilterByPriceQueryRequest request, CancellationToken cancellationToken)
        {
            var items = await _productService.FilterByPriceAsync(request.MinPrice, request.MaxPrice);
            return new FilterByPriceQueryResponse { Products = items };
        }
    }
}


