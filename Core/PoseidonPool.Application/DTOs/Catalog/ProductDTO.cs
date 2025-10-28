using System;

namespace PoseidonPool.Application.DTOs.Catalog
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public Guid? ProductImageId { get; set; }
        public Guid? ProductDetailId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
