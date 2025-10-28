namespace PoseidonPool.Application.DTOs.Order
{
    public class ListOrderDTO
    {
        public object Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderCode { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Completed { get; set; }
    }
}