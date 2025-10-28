using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductService _productService;

        public UpdateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.UpdateAsync(request.Id, request.Model);
            return new UpdateProductCommandResponse
            {
                Success = product != null,
                Product = product,
                Message = product != null ? "Product updated." : "Product not found."
            };
        }
    }
}
