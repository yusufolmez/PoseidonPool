using MediatR;
using PoseidonPool.Application.Features.Queries.Product.GetByCategory;
using System;

namespace PoseidonPool.Application.Features.Queries.Category.GetProducts
{
    public class GetProductsByCategoryIdQueryRequest : IRequest<GetProductsByCategoryIdQueryResponse>
    {
        public Guid CategoryId { get; set; }
    }
}


