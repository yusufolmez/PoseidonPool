using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Queries.Product.GetBySearch
{
    public class SearchProductsQueryResponse
    {
        public List<ProductDTO> Products { get; set; }
    }
}


