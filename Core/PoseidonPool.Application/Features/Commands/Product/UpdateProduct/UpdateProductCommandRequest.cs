using MediatR;
using PoseidonPool.Application.ViewModels.Catalog;
using Microsoft.AspNetCore.Http;

namespace PoseidonPool.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
    {
        public string? Id { get; set; }
        public VM_UpdateProduct Model { get; set; }
        public IFormFileCollection? Files { get; set; }
    }
}
