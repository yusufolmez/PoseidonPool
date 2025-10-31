using System.Threading.Tasks;
using PoseidonPool.Application.Abstractions.Payment;
using PoseidonPool.Application.DTOs.Payment;

namespace PoseidonPool.Application.Abstractions.Payment.Iyzico
{
    // Inherits generic payment operations; add Iyzi-specific capabilities here
    public interface IIyzicoService : IPaymentService
    {
        // Iyzi-specific: taksit seçenekleri (BIN ve fiyat üzerinden)
        Task<IyziInstallmentResponseDTO> RetrieveInstallmentsAsync(string binNumber, decimal price, string currency);

        // Iyzi-specific: ödeme detaylarını iyzico paymentId ile çekme
        Task<IyziPaymentDetailResponseDTO> RetrievePaymentDetailAsync(string paymentId);

        // Iyzi-specific: callback imzası/doğrulaması gerekiyorsa
        Task<bool> VerifyCallbackAsync(CallbackRequestDTO request);
    }
}


