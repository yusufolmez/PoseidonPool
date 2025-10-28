namespace PoseidonPool.Application.ViewModels.Catalog
{
    public class VM_CreateProduct
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public Guid? ProductImageId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }

        public VM_CreateProductDetail? ProductDetail { get; set; }
    }
}
