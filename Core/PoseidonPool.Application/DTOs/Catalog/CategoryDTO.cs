using System;

namespace PoseidonPool.Application.DTOs.Catalog
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
