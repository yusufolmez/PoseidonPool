using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Queries.Brand.GetAllBrands
{
    public class GetAllBrandsQueryResponse
    {
        public List<BrandDTO> Brands { get; set; }
    }
}
