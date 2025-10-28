using MediatR;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Features.Commands.Category.UpdateCategory
{
    public class UpdateCategoryCommandRequest : IRequest<UpdateCategoryCommandResponse>
    {
        public string Id { get; set; }
        public VM_UpdateCategory Model { get; set; }
    }
}
