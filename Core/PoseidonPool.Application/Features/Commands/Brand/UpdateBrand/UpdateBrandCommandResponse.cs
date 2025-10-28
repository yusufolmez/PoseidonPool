using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Commands.Brand.UpdateBrand
{
    public class UpdateBrandCommandResponse
    {
        public bool Success { get; set; }
        public BrandDTO Brand { get; set; }
        public string Message { get; set; }
    }
}
