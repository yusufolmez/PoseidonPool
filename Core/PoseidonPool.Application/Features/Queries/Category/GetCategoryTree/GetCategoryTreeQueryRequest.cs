using MediatR;
using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Queries.Category.GetCategoryTree
{
    public class GetCategoryTreeQueryRequest : IRequest<GetCategoryTreeQueryResponse>
    {
    }
}


