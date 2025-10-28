using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandResponse
    {
        public bool Success { get; set; }
        public ProductDTO Product { get; set; }
        public string Message { get; set; }
    }
}
