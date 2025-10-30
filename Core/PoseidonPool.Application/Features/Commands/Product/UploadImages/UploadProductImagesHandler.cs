using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Product.UploadImages
{
    public class UploadProductImagesHandler : IRequestHandler<UploadProductImagesCommandRequest, UploadProductImagesCommandResponse>
    {
        private readonly IProductService _productService;

        public UploadProductImagesHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<UploadProductImagesCommandResponse> Handle(UploadProductImagesCommandRequest request, CancellationToken cancellationToken)
        {
            var list = await _productService.AddImagesAsync(request.ProductId, request.Files);
            return new UploadProductImagesCommandResponse
            {
                Images = list.Select(x => new ImageSlot { Slot = x.slot, Url = x.url }).ToList()
            };
        }
    }
}


