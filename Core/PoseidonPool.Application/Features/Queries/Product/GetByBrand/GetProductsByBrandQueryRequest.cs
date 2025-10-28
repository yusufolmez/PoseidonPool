using MediatR;
using System;

namespace PoseidonPool.Application.Features.Queries.Product.GetByBrand
{
    public class GetProductsByBrandQueryRequest : IRequest<GetProductsByBrandQueryResponse>
    {
        public Guid BrandId { get; set; }
    }
}
