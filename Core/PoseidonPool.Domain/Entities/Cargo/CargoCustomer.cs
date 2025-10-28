﻿namespace PoseidonPool.Domain.Entities.Cargo
{
    public class CargoCustomer : BaseEntity
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string? UserCustomerId { get; set; }
    }
}
