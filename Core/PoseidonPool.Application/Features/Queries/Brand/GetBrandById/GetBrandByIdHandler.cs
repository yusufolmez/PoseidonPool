using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Brand.GetBrandById
{
    public class GetBrandByIdHandler : IRequestHandler<GetBrandByIdQueryRequest, GetBrandByIdQueryResponse>
    {
        private readonly IBrandService _brandService;

        public GetBrandByIdHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<GetBrandByIdQueryResponse> Handle(GetBrandByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var b = await _brandService.GetByIdAsync(request.Id);
            return new GetBrandByIdQueryResponse { Brand = b };
        }
    }
}
