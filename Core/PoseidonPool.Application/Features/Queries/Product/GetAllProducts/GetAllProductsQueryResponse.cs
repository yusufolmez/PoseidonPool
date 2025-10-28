using PoseidonPool.Application.DTOs.Catalog;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryResponse
    {
        public List<ProductDTO> Products { get; set; }
    }
}
