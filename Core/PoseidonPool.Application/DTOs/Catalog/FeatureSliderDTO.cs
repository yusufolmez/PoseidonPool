using System;

namespace PoseidonPool.Application.DTOs.Catalog
{
    public class FeatureSliderDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }
    }
}


