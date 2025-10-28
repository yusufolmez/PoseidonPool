using MediatR;

namespace PoseidonPool.Application.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductCommandRequest : IRequest<DeleteProductCommandResponse>
    {
        public string Id { get; set; }
    }
}
