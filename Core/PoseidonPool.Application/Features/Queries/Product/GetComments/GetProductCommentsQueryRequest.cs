using MediatR;

namespace PoseidonPool.Application.Features.Queries.Product.GetComments
{
    public class GetProductCommentsQueryRequest : IRequest<GetProductCommentsQueryResponse>
    {
        public string ProductId { get; set; }
    }
}


