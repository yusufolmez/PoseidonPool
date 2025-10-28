using MediatR;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Features.Commands.Brand.CreateBrand
{
    public class CreateBrandCommandRequest : IRequest<CreateBrandCommandResponse>
    {
        public VM_CreateBrand Model { get; set; }
    }
}
