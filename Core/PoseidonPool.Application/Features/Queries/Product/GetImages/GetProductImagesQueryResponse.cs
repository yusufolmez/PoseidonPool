using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Product.GetImages
{
    public class GetProductImagesQueryResponse
    {
        public List<ImageItem> Images { get; set; }
    }

    public class ImageItem
    {
        public string Slot { get; set; }
        public string Url { get; set; }
    }
}


