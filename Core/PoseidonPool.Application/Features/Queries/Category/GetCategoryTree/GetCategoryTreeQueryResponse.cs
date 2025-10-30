using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Queries.Category.GetCategoryTree
{
    public class GetCategoryTreeQueryResponse
    {
        public List<CategoryTreeDTO> Categories { get; set; }
    }
}


