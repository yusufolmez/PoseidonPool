using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Brand.GetAllBrands
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQueryRequest, GetAllBrandsQueryResponse>
    {
        private readonly IBrandService _brandService;

        public GetAllBrandsHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<GetAllBrandsQueryResponse> Handle(GetAllBrandsQueryRequest request, CancellationToken cancellationToken)
        {
            var brands = await _brandService.GetAllAsync();
            return new GetAllBrandsQueryResponse { Brands = brands };
        }
    }
}
