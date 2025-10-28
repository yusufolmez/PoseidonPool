using MediatR;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Features.Commands.Brand.UpdateBrand
{
    public class UpdateBrandCommandRequest : IRequest<UpdateBrandCommandResponse>
    {
        public string Id { get; set; }
        public VM_UpdateBrand Model { get; set; }
    }
}
