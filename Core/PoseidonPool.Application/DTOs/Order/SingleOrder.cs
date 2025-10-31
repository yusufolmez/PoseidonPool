using System;
using System.Collections.Generic;

namespace PoseidonPool.Application.DTOs.Order
{
    public class SingleOrderDTO
    {
        public object Id { get; set; }
        public AddressDTO Address { get; set; }
        public List<OrderItemDTO> BasketItems { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public bool Completed { get; set; }
        public int Status { get; set; } // OrderStatus enum value (0=Pending, 1=Paid, 2=Shipped, 3=Delivered, 4=Cancelled)
    }

    public class OrderItemDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}