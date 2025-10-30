using MediatR;

namespace PoseidonPool.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryRequest : IRequest<GetAllProductsQueryResponse>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        // e.g. "price_asc", "price_desc", "name_asc", "name_desc", "created_desc"
        public string Sort { get; set; }
    }
}
