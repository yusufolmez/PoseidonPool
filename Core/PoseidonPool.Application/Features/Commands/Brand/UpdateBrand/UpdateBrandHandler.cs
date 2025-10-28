using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Brand.UpdateBrand
{
    public class UpdateBrandHandler : IRequestHandler<UpdateBrandCommandRequest, UpdateBrandCommandResponse>
    {
        private readonly IBrandService _brandService;

        public UpdateBrandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<UpdateBrandCommandResponse> Handle(UpdateBrandCommandRequest request, CancellationToken cancellationToken)
        {
            var brand = await _brandService.UpdateAsync(request.Id, request.Model);
            return new UpdateBrandCommandResponse
            {
                Success = brand != null,
                Brand = brand,
                Message = brand != null ? "Brand updated." : "Brand not found."
            };
        }
    }
}
