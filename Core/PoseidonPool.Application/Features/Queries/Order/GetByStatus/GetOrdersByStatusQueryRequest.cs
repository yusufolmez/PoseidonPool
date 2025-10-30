using MediatR;

namespace PoseidonPool.Application.Features.Queries.Order.GetByStatus
{
    public class GetOrdersByStatusQueryRequest : IRequest<GetOrdersByStatusQueryResponse>
    {
        public string Status { get; set; }
    }
}


