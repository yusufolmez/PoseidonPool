using MediatR;

namespace PoseidonPool.Application.Features.Queries.Product.FilterByPrice
{
    public class FilterByPriceQueryRequest : IRequest<FilterByPriceQueryResponse>
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}


