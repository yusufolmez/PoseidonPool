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
    public class SpecialOfferService : ISpecialOfferService
    {
        private readonly ISpecialOfferReadRepository _read;
        private readonly ISpecialOfferWriteRepository _write;

        public SpecialOfferService(ISpecialOfferReadRepository read, ISpecialOfferWriteRepository write)
        {
            _read = read;
            _write = write;
        }

        public async Task<List<SpecialOfferDTO>> GetAllAsync()
        {
            var list = await _read.GetAll(false).OrderByDescending(s => s.CreatedDate).ToListAsync();
            return list.Select(Map).ToList();
        }

        public async Task<SpecialOfferDTO> GetByIdAsync(string id)
        {
            var e = await _read.GetByIdAsync(id, false);
            return e == null ? null : Map(e);
        }

        public async Task<SpecialOfferDTO> CreateAsync(SpecialOfferDTO model)
        {
            var e = new SpecialOffer
            {
                Id = System.Guid.NewGuid(),
                Title = model.Title,
                Subtitle = model.Subtitle,
                ImageUrl = model.ImageUrl
            };
            await _write.AddAsync(e);
            await _write.SaveAsync();
            return Map(e);
        }

        public async Task<SpecialOfferDTO> UpdateAsync(string id, SpecialOfferDTO model)
        {
            var e = await _read.GetByIdAsync(id);
            if (e == null) return null;
            if (!string.IsNullOrWhiteSpace(model.Title)) e.Title = model.Title;
            if (!string.IsNullOrWhiteSpace(model.Subtitle)) e.Subtitle = model.Subtitle;
            if (!string.IsNullOrWhiteSpace(model.ImageUrl)) e.ImageUrl = model.ImageUrl;
            _write.Update(e);
            await _write.SaveAsync();
            return Map(e);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var ok = await _write.RemoveAsync(id);
            if (!ok) return false;
            await _write.SaveAsync();
            return true;
        }

        private static SpecialOfferDTO Map(SpecialOffer s)
        {
            return new SpecialOfferDTO
            {
                Id = s.Id,
                Title = s.Title,
                Subtitle = s.Subtitle,
                ImageUrl = s.ImageUrl
            };
        }
    }
}


