using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Product.AddComment
{
    public class AddProductCommentHandler : IRequestHandler<AddProductCommentCommandRequest, AddProductCommentCommandResponse>
    {
        private readonly IProductService _productService;

        public AddProductCommentHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<AddProductCommentCommandResponse> Handle(AddProductCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var dto = await _productService.AddCommentAsync(request.ProductId, request.Model);
            return new AddProductCommentCommandResponse { Comment = dto };
        }
    }
}


