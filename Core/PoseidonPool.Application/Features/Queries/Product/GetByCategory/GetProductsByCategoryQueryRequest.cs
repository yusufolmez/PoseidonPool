using MediatR;
using System;

namespace PoseidonPool.Application.Features.Queries.Product.GetByCategory
{
    public class GetProductsByCategoryQueryRequest : IRequest<GetProductsByCategoryQueryResponse>
    {
        public Guid CategoryId { get; set; }
    }
}
