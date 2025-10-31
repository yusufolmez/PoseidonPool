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
    public class OfferDiscountService : IOfferDiscountService
    {
        private readonly IOfferDiscountReadRepository _read;
        private readonly IOfferDiscountWriteRepository _write;

        public OfferDiscountService(IOfferDiscountReadRepository read, IOfferDiscountWriteRepository write)
        {
            _read = read;
            _write = write;
        }

        public async Task<List<OfferDiscountDTO>> GetAllAsync()
        {
            var list = await _read.GetAll(false).OrderByDescending(o => o.CreatedDate).ToListAsync();
            return list.Select(Map).ToList();
        }

        public async Task<OfferDiscountDTO> GetByIdAsync(string id)
        {
            var e = await _read.GetByIdAsync(id, false);
            return e == null ? null : Map(e);
        }

        public async Task<OfferDiscountDTO> CreateAsync(OfferDiscountDTO model)
        {
            var e = new OfferDiscount
            {
                Id = System.Guid.NewGuid(),
                Title = model.Title,
                SubTitle = model.SubTitle,
                ImageUrl = model.ImageUrl,
                ButtonTitle = model.ButtonTitle
            };
            await _write.AddAsync(e);
            await _write.SaveAsync();
            return Map(e);
        }

        public async Task<OfferDiscountDTO> UpdateAsync(string id, OfferDiscountDTO model)
        {
            var e = await _read.GetByIdAsync(id);
            if (e == null) return null;
            if (!string.IsNullOrWhiteSpace(model.Title)) e.Title = model.Title;
            if (!string.IsNullOrWhiteSpace(model.SubTitle)) e.SubTitle = model.SubTitle;
            if (!string.IsNullOrWhiteSpace(model.ImageUrl)) e.ImageUrl = model.ImageUrl;
            if (!string.IsNullOrWhiteSpace(model.ButtonTitle)) e.ButtonTitle = model.ButtonTitle;
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

        private static OfferDiscountDTO Map(OfferDiscount o)
        {
            return new OfferDiscountDTO
            {
                Id = o.Id,
                Title = o.Title,
                SubTitle = o.SubTitle,
                ImageUrl = o.ImageUrl,
                ButtonTitle = o.ButtonTitle
            };
        }
    }
}


