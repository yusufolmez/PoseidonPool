using System.ComponentModel.DataAnnotations;
using MediatR;

namespace PoseidonPool.Application.Features.Commands.ProductLike.Remove
{
    public class RemoveProductLikeCommandRequest : IRequest<RemoveProductLikeCommandResponse>
    {
        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductId { get; set; }
    }
}

