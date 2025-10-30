using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Product.DeleteImage
{
    public class DeleteProductImageHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        private readonly IProductService _productService;

        public DeleteProductImageHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var ok = await _productService.DeleteImageAsync(request.ProductId, request.SlotOrKey);
            return new DeleteProductImageCommandResponse { Success = ok };
        }
    }
}


