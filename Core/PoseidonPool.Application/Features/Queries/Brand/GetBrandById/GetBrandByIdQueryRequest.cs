using MediatR;

namespace PoseidonPool.Application.Features.Queries.Brand.GetBrandById
{
    public class GetBrandByIdQueryRequest : IRequest<GetBrandByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}
