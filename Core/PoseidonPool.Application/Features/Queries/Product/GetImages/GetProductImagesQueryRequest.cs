using MediatR;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Product.GetImages
{
    public class GetProductImagesQueryRequest : IRequest<GetProductImagesQueryResponse>
    {
        public string ProductId { get; set; }
    }
}


