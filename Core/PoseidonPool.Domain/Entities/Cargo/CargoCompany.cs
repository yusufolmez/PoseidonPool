namespace PoseidonPool.Domain.Entities.Cargo
{
    public class CargoCompany : BaseEntity
    {
        public string CargoCompanyName { get; set; }
        public ICollection<CargoDetail> CargoDetails { get; set; }
    }
}
