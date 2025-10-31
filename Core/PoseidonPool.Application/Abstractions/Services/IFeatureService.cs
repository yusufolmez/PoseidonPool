using System.Collections.Generic;
using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IFeatureService
    {
        Task<List<FeatureDTO>> GetAllAsync();
        Task<FeatureDTO> GetByIdAsync(string id);
        Task<FeatureDTO> CreateAsync(FeatureDTO model);
        Task<FeatureDTO> UpdateAsync(string id, FeatureDTO model);
        Task<bool> DeleteAsync(string id);
    }
}


