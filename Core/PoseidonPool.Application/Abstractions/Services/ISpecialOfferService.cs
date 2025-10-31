using System.Collections.Generic;
using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface ISpecialOfferService
    {
        Task<List<SpecialOfferDTO>> GetAllAsync();
        Task<SpecialOfferDTO> GetByIdAsync(string id);
        Task<SpecialOfferDTO> CreateAsync(SpecialOfferDTO model);
        Task<SpecialOfferDTO> UpdateAsync(string id, SpecialOfferDTO model);
        Task<bool> DeleteAsync(string id);
    }
}


