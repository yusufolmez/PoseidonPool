using System.Collections.Generic;
using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IFeatureSliderService
    {
        Task<List<FeatureSliderDTO>> GetAllAsync(bool? status = null);
        Task<FeatureSliderDTO> GetByIdAsync(string id);
        Task<FeatureSliderDTO> CreateAsync(FeatureSliderDTO model);
        Task<FeatureSliderDTO> UpdateAsync(string id, FeatureSliderDTO model);
        Task<bool> DeleteAsync(string id);
        Task<bool> SetStatusAsync(string id, bool status);
    }
}


