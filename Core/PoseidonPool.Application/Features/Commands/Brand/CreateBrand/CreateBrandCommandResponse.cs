using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Commands.Brand.CreateBrand
{
    public class CreateBrandCommandResponse
    {
        public bool Success { get; set; }
        public BrandDTO Brand { get; set; }
        public string Message { get; set; }
    }
}
