using PoseidonPool.Application.DTOs.Catalog;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Product.GetByBrand
{
    public class GetProductsByBrandQueryResponse
    {
        public List<ProductDTO> Products { get; set; }
    }
}
