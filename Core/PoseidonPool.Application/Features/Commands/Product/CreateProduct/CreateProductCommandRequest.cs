using MediatR;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public VM_CreateProduct Model { get; set; }
    }
}
