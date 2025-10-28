using MediatR;

namespace PoseidonPool.Application.Features.Commands.Brand.DeleteBrand
{
    public class DeleteBrandCommandRequest : IRequest<DeleteBrandCommandResponse>
    {
        public string Id { get; set; }
    }
}
