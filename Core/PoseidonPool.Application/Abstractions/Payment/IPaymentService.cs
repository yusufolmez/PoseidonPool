using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Payment;

namespace PoseidonPool.Application.Abstractions.Payment
{
    public interface IPaymentService
    {
        Task<CreatePaymentResponseDTO> CreateAsync(CreatePaymentRequestDTO request);
        Task<CallbackResponseDTO> HandleCallbackAsync(CallbackRequestDTO request);
        Task<RefundResponseDTO> RefundAsync(RefundRequestDTO request);
        Task<CancelResponseDTO> CancelAsync(CancelRequestDTO request);
    }
}


