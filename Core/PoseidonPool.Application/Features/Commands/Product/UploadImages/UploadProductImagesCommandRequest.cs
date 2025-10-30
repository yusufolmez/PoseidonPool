using MediatR;
using Microsoft.AspNetCore.Http;

namespace PoseidonPool.Application.Features.Commands.Product.UploadImages
{
    public class UploadProductImagesCommandRequest : IRequest<UploadProductImagesCommandResponse>
    {
        public string ProductId { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}


