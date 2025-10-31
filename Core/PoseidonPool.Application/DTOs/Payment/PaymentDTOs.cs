using System.Collections.Generic;

namespace PoseidonPool.Application.DTOs.Payment
{
    public class CreatePaymentRequestDTO
    {
        public decimal Price { get; set; }
        public decimal PaidPrice { get; set; }
        public string Currency { get; set; } = "TRY";
        public string ConversationId { get; set; }
        public string CallbackUrl { get; set; }
        public BuyerDTO Buyer { get; set; }
        public List<BasketItemDTO> Items { get; set; }
    }

    public class CreatePaymentResponseDTO
    {
        public string Status { get; set; }
        public string RedirectUrl { get; set; }
        public string CheckoutFormContent { get; set; }
        public string ConversationId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CallbackRequestDTO
    {
        public string Token { get; set; }
        public string ConversationId { get; set; }
    }

    public class CallbackResponseDTO
    {
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RefundRequestDTO
    {
        public string PaymentTransactionId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";
        public string Reason { get; set; }
        public string Ip { get; set; }
    }

    public class RefundResponseDTO
    {
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CancelRequestDTO
    {
        public string PaymentId { get; set; }
        public string Ip { get; set; }
    }

    public class CancelResponseDTO
    {
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class BuyerDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string GsmNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string Ip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }

    public class BasketItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public decimal Price { get; set; }
    }
}


