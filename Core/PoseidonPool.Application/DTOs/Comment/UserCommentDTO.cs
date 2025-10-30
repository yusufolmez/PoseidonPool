using System;

namespace PoseidonPool.Application.DTOs.Comment
{
    public class UserCommentDTO
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public string ImageUrl { get; set; }
        public string CommentDetail { get; set; }
        public int Rating { get; set; }
        public bool Status { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}


