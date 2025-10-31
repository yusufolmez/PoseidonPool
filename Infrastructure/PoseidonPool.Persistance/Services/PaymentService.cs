using System.Threading.Tasks;
using PoseidonPool.Application.Abstractions.Payment;
using PoseidonPool.Application.Abstractions.Payment.Iyzico;
using PoseidonPool.Application.DTOs.Payment;

namespace PoseidonPool.Persistance.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IIyzicoService _iyzico;
        public PaymentService(IIyzicoService iyzico)
        {
            _iyzico = iyzico;
        }

        public Task<CreatePaymentResponseDTO> CreateAsync(CreatePaymentRequestDTO request) => _iyzico.CreateAsync(request);
        public Task<CallbackResponseDTO> HandleCallbackAsync(CallbackRequestDTO request) => _iyzico.HandleCallbackAsync(request);
        public Task<RefundResponseDTO> RefundAsync(RefundRequestDTO request) => _iyzico.RefundAsync(request);
        public Task<CancelResponseDTO> CancelAsync(CancelRequestDTO request) => _iyzico.CancelAsync(request);
    }
}


