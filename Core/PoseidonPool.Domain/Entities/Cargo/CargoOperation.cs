namespace PoseidonPool.Domain.Entities.Cargo
{
    public class CargoOperation : BaseEntity
    {
        public int CargoOperationId { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
