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
            if (request.Page.HasValue || request.PageSize.HasValue || !string.IsNullOrWhiteSpace(request.Sort))
            {
                var page = request.Page ?? 1;
                var pageSize = request.PageSize ?? 20;
                var (items, total) = await _productService.GetAllPagedAsync(page, pageSize, request.Sort);
                return new GetAllProductsQueryResponse
                {
                    Products = items,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }

            var products = await _productService.GetAllAsync();
            return new GetAllProductsQueryResponse { Products = products };
        }
    }
}
