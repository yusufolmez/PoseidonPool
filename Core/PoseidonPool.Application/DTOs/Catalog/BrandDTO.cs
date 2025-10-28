using System;

namespace PoseidonPool.Application.DTOs.Catalog
{
    public class BrandDTO
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
