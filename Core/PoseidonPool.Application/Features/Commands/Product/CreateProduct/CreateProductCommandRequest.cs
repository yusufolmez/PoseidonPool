using MediatR;
using PoseidonPool.Application.ViewModels.Catalog;
using Microsoft.AspNetCore.Http;

namespace PoseidonPool.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public VM_CreateProduct Model { get; set; }
        // Optional uploaded files (multipart/form-data)
        public IFormFileCollection Files { get; set; }
    }
}
