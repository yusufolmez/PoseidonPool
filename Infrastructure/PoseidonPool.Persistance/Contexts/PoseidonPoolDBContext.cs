using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Domain.Entities.Basket;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Domain.Entities.Identity;
using PoseidonPool.Domain.Entities.Order;

namespace PoseidonPool.Persistance.Contexts
{
    public class PoseidonPoolDBContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public PoseidonPoolDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Ordering> Orderings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Ordering>()
                .HasKey(o => o.Id);

            builder.Entity<Ordering>()
                .HasIndex(o => o.OrderingId)
                .IsUnique();

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Basket)
                .HasForeignKey<Ordering>(o => o.Id);

            base.OnModelCreating(builder);
        }
    }
}
