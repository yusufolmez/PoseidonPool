using PoseidonPool.Application.DTOs.Catalog;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.ProductLike.GetUserLikes
{
    public class GetUserLikesQueryResponse
    {
        public List<ProductLikeDTO> Likes { get; set; }
    }
}

