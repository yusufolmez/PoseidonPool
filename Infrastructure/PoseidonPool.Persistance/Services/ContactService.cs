using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseidonPool.Persistance.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactReadRepository _read;
        private readonly IContactWriteRepository _write;

        public ContactService(IContactReadRepository read, IContactWriteRepository write)
        {
            _read = read;
            _write = write;
        }

        public async Task<List<ContactDTO>> GetAllAsync(bool? isRead = null)
        {
            var q = _read.GetAll(false);
            if (isRead.HasValue) q = q.Where(c => c.IsRead == isRead.Value);
            var list = await q.OrderByDescending(c => c.SendDate).ToListAsync();
            return list.Select(Map).ToList();
        }

        public async Task<ContactDTO> GetByIdAsync(string id)
        {
            var c = await _read.GetByIdAsync(id, false);
            return c == null ? null : Map(c);
        }

        public async Task<ContactDTO> CreateAsync(ContactDTO model)
        {
            var entity = new Contact
            {
                Id = System.Guid.NewGuid(),
                NameSurname = model.NameSurname,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                IsRead = false,
                SendDate = System.DateTime.UtcNow
            };
            await _write.AddAsync(entity);
            await _write.SaveAsync();
            return Map(entity);
        }

        public async Task<ContactDTO> MarkReadAsync(string id, bool isRead)
        {
            var c = await _read.GetByIdAsync(id);
            if (c == null) return null;
            c.IsRead = isRead;
            _write.Update(c);
            await _write.SaveAsync();
            return Map(c);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var ok = await _write.RemoveAsync(id);
            if (!ok) return false;
            await _write.SaveAsync();
            return true;
        }

        private static ContactDTO Map(Contact c)
        {
            return new ContactDTO
            {
                Id = c.Id,
                NameSurname = c.NameSurname,
                Email = c.Email,
                Subject = c.Subject,
                Message = c.Message,
                IsRead = c.IsRead,
                SendDate = c.SendDate
            };
        }
    }
}


