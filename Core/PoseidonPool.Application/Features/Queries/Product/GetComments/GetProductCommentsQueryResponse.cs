using System.Collections.Generic;
using PoseidonPool.Application.DTOs.Comment;

namespace PoseidonPool.Application.Features.Queries.Product.GetComments
{
    public class GetProductCommentsQueryResponse
    {
        public List<UserCommentDTO> Comments { get; set; }
    }
}


