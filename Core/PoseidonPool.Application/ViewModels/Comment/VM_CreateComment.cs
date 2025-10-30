namespace PoseidonPool.Application.ViewModels.Comment
{
    public class VM_CreateComment
    {
        public string CustomerId { get; set; }
        public string? ImageUrl { get; set; }
        public string CommentDetail { get; set; }
        public int Rating { get; set; }
        public bool Status { get; set; } = true;
    }
}


