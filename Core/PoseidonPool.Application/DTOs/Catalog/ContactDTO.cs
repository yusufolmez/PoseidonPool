using System;

namespace PoseidonPool.Application.DTOs.Catalog
{
    public class ContactDTO
    {
        public Guid Id { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime SendDate { get; set; }
    }
}


