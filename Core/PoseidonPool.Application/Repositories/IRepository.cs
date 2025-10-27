using Microsoft.EntityFrameworkCore;
using PoseidonPool.Domain.Entities;

namespace PoseidonPool.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
