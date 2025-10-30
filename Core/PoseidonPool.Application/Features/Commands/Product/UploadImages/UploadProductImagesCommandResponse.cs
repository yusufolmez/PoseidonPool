using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Commands.Product.UploadImages
{
    public class UploadProductImagesCommandResponse
    {
        public List<ImageSlot> Images { get; set; }
    }

    public class ImageSlot
    {
        public string Slot { get; set; }
        public string Url { get; set; }
    }
}


