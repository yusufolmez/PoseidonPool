namespace PoseidonPool.Domain.Entities.Cargo
{
    public class CargoOperation : BaseEntity
    {
        public Guid CargoDetailId { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public DateTime OperationDate { get; set; }
        public CargoDetail CargoDetail { get; set; }
    }
}
