using System.Collections.Generic;
using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IOfferDiscountService
    {
        Task<List<OfferDiscountDTO>> GetAllAsync();
        Task<OfferDiscountDTO> GetByIdAsync(string id);
        Task<OfferDiscountDTO> CreateAsync(OfferDiscountDTO model);
        Task<OfferDiscountDTO> UpdateAsync(string id, OfferDiscountDTO model);
        Task<bool> DeleteAsync(string id);
    }
}


