using PoseidonPool.Application.DTOs.Catalog;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryResponse
    {
        public List<ProductDTO> Products { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? TotalCount { get; set; }
    }
}
