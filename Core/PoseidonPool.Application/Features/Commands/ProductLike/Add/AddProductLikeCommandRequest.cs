using System.ComponentModel.DataAnnotations;
using MediatR;

namespace PoseidonPool.Application.Features.Commands.ProductLike.Add
{
    public class AddProductLikeCommandRequest : IRequest<AddProductLikeCommandResponse>
    {
        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductId { get; set; }
    }
}

