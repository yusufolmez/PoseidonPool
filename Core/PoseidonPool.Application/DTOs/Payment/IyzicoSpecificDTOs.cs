using System.Collections.Generic;

namespace PoseidonPool.Application.DTOs.Payment
{
    public class IyziInstallmentResponseDTO
    {
        public string BinNumber { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public List<IyziInstallmentOptionDTO> Options { get; set; }
    }

    public class IyziInstallmentOptionDTO
    {
        public int InstallmentNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal InstallmentPrice { get; set; }
    }

    public class IyziPaymentDetailResponseDTO
    {
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}


