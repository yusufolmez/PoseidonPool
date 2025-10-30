using MediatR;

namespace PoseidonPool.Application.Features.Queries.Product.GetBySearch
{
    public class SearchProductsQueryRequest : IRequest<SearchProductsQueryResponse>
    {
        public string Query { get; set; }
    }
}


