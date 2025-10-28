using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Brand.DeleteBrand
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommandRequest, DeleteBrandCommandResponse>
    {
        private readonly IBrandService _brandService;

        public DeleteBrandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<DeleteBrandCommandResponse> Handle(DeleteBrandCommandRequest request, CancellationToken cancellationToken)
        {
            var success = await _brandService.DeleteAsync(request.Id);
            return new DeleteBrandCommandResponse
            {
                Success = success,
                Message = success ? "Brand deleted." : "Brand not found."
            };
        }
    }
}
