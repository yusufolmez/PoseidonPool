using MediatR;
using PoseidonPool.Application.ViewModels.Catalog;

namespace PoseidonPool.Application.Features.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandRequest : IRequest<CreateCategoryCommandResponse>
    {
        public VM_CreateCategory Model { get; set; }
    }
}
