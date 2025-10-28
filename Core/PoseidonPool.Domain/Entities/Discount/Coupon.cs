namespace PoseidonPool.Domain.Entities.Discount
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; }
        public int Rate { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidDate { get; set; }
    }
}
