using System.ComponentModel.DataAnnotations;
using MediatR;

namespace PoseidonPool.Application.Features.Queries.ProductLike.CheckProductLike
{
    public class CheckProductLikeQueryRequest : IRequest<CheckProductLikeQueryResponse>
    {
        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductId { get; set; }
    }
}

