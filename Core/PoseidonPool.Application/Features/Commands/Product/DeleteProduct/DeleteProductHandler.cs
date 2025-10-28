using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IProductService _productService;

        public DeleteProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var success = await _productService.DeleteAsync(request.Id);
            return new DeleteProductCommandResponse
            {
                Success = success,
                Message = success ? "Product deleted." : "Product not found."
            };
        }
    }
}
