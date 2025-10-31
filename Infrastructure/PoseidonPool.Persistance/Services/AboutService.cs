using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using System.Linq;
using System.Threading.Tasks;

namespace PoseidonPool.Persistance.Services
{
    public class AboutService : IAboutService
    {
        private readonly IAboutReadRepository _read;
        private readonly IAboutWriteRepository _write;

        public AboutService(IAboutReadRepository read, IAboutWriteRepository write)
        {
            _read = read;
            _write = write;
        }

        public async Task<AboutDTO> GetAsync()
        {
            var about = await _read.GetAll(false).FirstOrDefaultAsync();
            if (about == null)
            {
                return new AboutDTO();
            }
            return Map(about);
        }

        public async Task<AboutDTO> UpdateAsync(AboutDTO model)
        {
            var about = await _read.GetAll().FirstOrDefaultAsync();
            if (about == null)
            {
                about = new About
                {
                    Id = System.Guid.NewGuid(),
                    Description = model.Description,
                    Address = model.Address,
                    Email = model.Email,
                    Phone = model.Phone
                };
                await _write.AddAsync(about);
            }
            else
            {
                about.Description = model.Description;
                about.Address = model.Address;
                about.Email = model.Email;
                about.Phone = model.Phone;
                _write.Update(about);
            }
            await _write.SaveAsync();
            return Map(about);
        }

        private static AboutDTO Map(About a)
        {
            return new AboutDTO
            {
                Description = a.Description,
                Address = a.Address,
                Email = a.Email,
                Phone = a.Phone
            };
        }
    }
}


