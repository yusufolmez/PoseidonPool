using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Brand.CreateBrand
{
    public class CreateBrandHandler : IRequestHandler<CreateBrandCommandRequest, CreateBrandCommandResponse>
    {
        private readonly IBrandService _brandService;

        public CreateBrandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<CreateBrandCommandResponse> Handle(CreateBrandCommandRequest request, CancellationToken cancellationToken)
        {
            var brand = await _brandService.CreateAsync(request.Model);
            return new CreateBrandCommandResponse
            {
                Success = true,
                Brand = brand,
                Message = "Brand created."
            };
        }
    }
}
