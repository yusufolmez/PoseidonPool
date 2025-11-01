using System.ComponentModel.DataAnnotations;
using MediatR;

namespace PoseidonPool.Application.Features.Queries.ProductLike.GetProductLikes
{
    public class GetProductLikesQueryRequest : IRequest<GetProductLikesQueryResponse>
    {
        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductId { get; set; }
    }
}

