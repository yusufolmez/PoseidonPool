using System.Collections.Generic;
using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IContactService
    {
        Task<List<ContactDTO>> GetAllAsync(bool? isRead = null);
        Task<ContactDTO> GetByIdAsync(string id);
        Task<ContactDTO> CreateAsync(ContactDTO model);
        Task<ContactDTO> MarkReadAsync(string id, bool isRead);
        Task<bool> DeleteAsync(string id);
    }
}


