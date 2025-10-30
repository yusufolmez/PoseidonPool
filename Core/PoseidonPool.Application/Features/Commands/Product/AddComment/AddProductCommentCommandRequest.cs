using MediatR;
using PoseidonPool.Application.DTOs.Comment;
using PoseidonPool.Application.ViewModels.Comment;

namespace PoseidonPool.Application.Features.Commands.Product.AddComment
{
    public class AddProductCommentCommandRequest : IRequest<AddProductCommentCommandResponse>
    {
        public string ProductId { get; set; }
        public VM_CreateComment Model { get; set; }
    }
}


