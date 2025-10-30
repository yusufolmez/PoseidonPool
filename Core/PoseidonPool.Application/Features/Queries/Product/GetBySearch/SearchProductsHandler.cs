using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Product.GetBySearch
{
    public class SearchProductsHandler : IRequestHandler<SearchProductsQueryRequest, SearchProductsQueryResponse>
    {
        private readonly IProductService _productService;

        public SearchProductsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<SearchProductsQueryResponse> Handle(SearchProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var items = await _productService.SearchAsync(request.Query);
            return new SearchProductsQueryResponse { Products = items };
        }
    }
}


