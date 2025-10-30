using MediatR;

namespace PoseidonPool.Application.Features.Commands.Product.DeleteImage
{
    public class DeleteProductImageCommandRequest : IRequest<DeleteProductImageCommandResponse>
    {
        public string ProductId { get; set; }
        public string SlotOrKey { get; set; }
    }
}
