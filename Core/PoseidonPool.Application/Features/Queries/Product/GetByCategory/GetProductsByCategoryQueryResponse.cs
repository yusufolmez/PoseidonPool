using PoseidonPool.Application.DTOs.Catalog;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Product.GetByCategory
{
    public class GetProductsByCategoryQueryResponse
    {
        public List<ProductDTO> Products { get; set; }
    }
}
