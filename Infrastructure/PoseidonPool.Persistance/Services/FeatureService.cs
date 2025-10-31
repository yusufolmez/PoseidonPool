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
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureReadRepository _read;
        private readonly IFeatureWriteRepository _write;

        public FeatureService(IFeatureReadRepository read, IFeatureWriteRepository write)
        {
            _read = read;
            _write = write;
        }

        public async Task<List<FeatureDTO>> GetAllAsync()
        {
            var list = await _read.GetAll(false).OrderByDescending(f => f.CreatedDate).ToListAsync();
            return list.Select(Map).ToList();
        }

        public async Task<FeatureDTO> GetByIdAsync(string id)
        {
            var e = await _read.GetByIdAsync(id, false);
            return e == null ? null : Map(e);
        }

        public async Task<FeatureDTO> CreateAsync(FeatureDTO model)
        {
            var e = new Feature { Id = System.Guid.NewGuid(), Title = model.Title, Icon = model.Icon };
            await _write.AddAsync(e);
            await _write.SaveAsync();
            return Map(e);
        }

        public async Task<FeatureDTO> UpdateAsync(string id, FeatureDTO model)
        {
            var e = await _read.GetByIdAsync(id);
            if (e == null) return null;
            if (!string.IsNullOrWhiteSpace(model.Title)) e.Title = model.Title;
            if (!string.IsNullOrWhiteSpace(model.Icon)) e.Icon = model.Icon;
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

        private static FeatureDTO Map(Feature f)
        {
            return new FeatureDTO { Id = f.Id, Title = f.Title, Icon = f.Icon };
        }
    }
}


