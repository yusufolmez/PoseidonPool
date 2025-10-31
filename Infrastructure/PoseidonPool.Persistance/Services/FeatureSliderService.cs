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
    public class FeatureSliderService : IFeatureSliderService
    {
        private readonly IFeatureSliderReadRepository _read;
        private readonly IFeatureSliderWriteRepository _write;

        public FeatureSliderService(IFeatureSliderReadRepository read, IFeatureSliderWriteRepository write)
        {
            _read = read;
            _write = write;
        }

        public async Task<List<FeatureSliderDTO>> GetAllAsync(bool? status = null)
        {
            var q = _read.GetAll(false);
            if (status.HasValue) q = q.Where(s => s.Status == status.Value);
            var list = await q.OrderByDescending(s => s.CreatedDate).ToListAsync();
            return list.Select(Map).ToList();
        }

        public async Task<FeatureSliderDTO> GetByIdAsync(string id)
        {
            var e = await _read.GetByIdAsync(id, false);
            return e == null ? null : Map(e);
        }

        public async Task<FeatureSliderDTO> CreateAsync(FeatureSliderDTO model)
        {
            var e = new FeatureSlider
            {
                Id = System.Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Status = model.Status
            };
            await _write.AddAsync(e);
            await _write.SaveAsync();
            return Map(e);
        }

        public async Task<FeatureSliderDTO> UpdateAsync(string id, FeatureSliderDTO model)
        {
            var e = await _read.GetByIdAsync(id);
            if (e == null) return null;
            if (!string.IsNullOrWhiteSpace(model.Title)) e.Title = model.Title;
            if (!string.IsNullOrWhiteSpace(model.Description)) e.Description = model.Description;
            if (!string.IsNullOrWhiteSpace(model.ImageUrl)) e.ImageUrl = model.ImageUrl;
            e.Status = model.Status;
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

        public async Task<bool> SetStatusAsync(string id, bool status)
        {
            var e = await _read.GetByIdAsync(id);
            if (e == null) return false;
            e.Status = status;
            _write.Update(e);
            await _write.SaveAsync();
            return true;
        }

        private static FeatureSliderDTO Map(FeatureSlider s)
        {
            return new FeatureSliderDTO
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                Status = s.Status
            };
        }
    }
}


