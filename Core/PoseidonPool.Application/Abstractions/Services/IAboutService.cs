using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IAboutService
    {
        Task<AboutDTO> GetAsync();
        Task<AboutDTO> UpdateAsync(AboutDTO model);
    }
}


