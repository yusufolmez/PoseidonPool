using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Queries.Category.GetProducts
{
    public class GetProductsByCategoryIdQueryResponse
    {
        public List<ProductDTO> Products { get; set; }
    }
}


