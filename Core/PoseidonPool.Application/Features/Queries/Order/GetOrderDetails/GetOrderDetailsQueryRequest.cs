using MediatR;

namespace PoseidonPool.Application.Features.Queries.Order.GetOrderDetails
{
    public class GetOrderDetailsQueryRequest : IRequest<GetOrderDetailsQueryResponse>
    {
        public string Id { get; set; }
    }
}


