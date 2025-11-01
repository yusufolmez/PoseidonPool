namespace PoseidonPool.Application.DTOs.Catalog
{
    public class ProductLikeDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

