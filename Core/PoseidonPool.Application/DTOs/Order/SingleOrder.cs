namespace PoseidonPool.Application.DTOs.Order
{
    public class SingleOrderDTO
    {
        public object Id { get; set; }
        public AddressDTO Address { get; set; }
        public object BasketItems { get; set; }
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public bool Completed { get; set; }
    }
}