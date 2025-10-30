using System;
using System.Collections.Generic;

namespace PoseidonPool.Application.DTOs.Catalog
{
    public class CategoryTreeDTO
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public Guid? ParentId { get; set; }
        public List<CategoryTreeDTO> Children { get; set; } = new List<CategoryTreeDTO>();
    }
}


