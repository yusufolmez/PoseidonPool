using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Queries.Product.FilterByPrice
{
    public class FilterByPriceQueryResponse
    {
        public List<ProductDTO> Products { get; set; }
    }
}


