using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Domain.Entities;
using PoseidonPool.Domain.Entities.Basket;
using PoseidonPool.Domain.Entities.Cargo; 
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Domain.Entities.Comment;
using PoseidonPool.Domain.Entities.Discount; 
using PoseidonPool.Domain.Entities.Identity;
//using PoseidonPool.Domain.Entities.Message; 
using PoseidonPool.Domain.Entities.Order;

namespace PoseidonPool.Persistance.Contexts
{
    public class PoseidonPoolDBContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public PoseidonPoolDBContext(DbContextOptions options) : base(options)
        { }

        // -----------------------------------------------------------------
        // ## 1. DbSet Tanımlamaları (Entity'leri Veritabanına Yansıtma)
        // -----------------------------------------------------------------

        // BASKET
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        // CARGO
        public DbSet<CargoCompany> CargoCompanies { get; set; }
        public DbSet<CargoCustomer> CargoCustomers { get; set; }
        public DbSet<CargoDetail> CargoDetails { get; set; }
        public DbSet<CargoOperation> CargoOperations { get; set; }

        // CATALOG
        public DbSet<About> Abouts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureSlider> FeatureSliders { get; set; }
        public DbSet<OfferDiscount> OfferDiscounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<ProductLike> ProductLikes { get; set; }

        // COMMENT
        public DbSet<UserComment> UserComments { get; set; }

        // DISCOUNT
        public DbSet<Coupon> Coupons { get; set; }

        // MESSAGE
        //public DbSet<Message> Messages { get; set; }

        // ORDER
        public DbSet<Ordering> Orderings { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        // Address.cs bir Value Object olduğu için genellikle DbSet olarak tanımlanmaz.
        // OnModelCreating'de "Owned Entity" olarak konfigüre edilir.
        //public DbSet<Payment> Payments { get; set; }

        // -----------------------------------------------------------------
        // ## 2. İlişki ve Model Konfigürasyonları
        // -----------------------------------------------------------------

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // -------------------------------------------------------------
            // A. ORDER (Sipariş) İlişkileri
            // -------------------------------------------------------------

            // 1. Ordering - OrderDetail (Bire Çok)
            builder.Entity<Ordering>()
                .HasMany(o => o.OrderDetails) // Ordering içinde OrderDetails koleksiyonu olmalı (List/ICollection)
                .WithOne(od => od.Ordering)
                .HasForeignKey(od => od.OrderingId)
                .OnDelete(DeleteBehavior.Cascade); // Sipariş silinirse detayları da silinsin

            // 2. Ordering - Address (Owned Entity / Value Object)
            // Address ayrı bir tablo oluşturmaz, Ordering tablosuna gömülür.
            builder.Entity<Ordering>().OwnsOne(o => o.ShippingAddress); // Sadece .OwnsOne(o => o.Address) yeterlidir.

            // 3. Ordering - CargoDetail (Bire Bir)
            builder.Entity<Ordering>()
                .HasOne<CargoDetail>()
                .WithOne(cd => cd.Ordering)
                .HasForeignKey<CargoDetail>(cd => cd.OrderingId); // CargoDetail tablosunda OrderId Foreign Key olarak tutulur

            // 3. Ordering - Payment (Bire Bir) - (Payment DbSet'i eklenmiş olmalı)





            // -------------------------------------------------------------
            // B. CATALOG (Katalog) İlişkileri
            // -------------------------------------------------------------

            // 5. Category - Product (Bire Çok)
            builder.Entity<Category>()
                .HasMany(c => c.Products) // Category içinde Products koleksiyonu olmalı
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            // 6. Brand - Product (Bire Çok)
            builder.Entity<Brand>()
                .HasMany(b => b.Products) // Brand içinde Products koleksiyonu olmalı
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId);

            // 7. Product - ProductImage (Bire Çok)
            builder.Entity<Product>()
                .HasMany(p => p.Images) // Product içinde Images koleksiyonu olmalı
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Ürün silinirse görselleri de silinsin

            // 8. Product - ProductDetail (Bire Bir)
            builder.Entity<Product>()
                .HasOne(p => p.ProductDetail)
                .WithOne(pd => pd.Product)
                .HasForeignKey<ProductDetail>(pd => pd.ProductId); // ProductDetail içinde ProductId Foreign Key olarak tutulur

            // -------------------------------------------------------------
            // C. USER, BASKET & COMMENT (Kullanıcı, Sepet ve Yorum) İlişkileri
            // -------------------------------------------------------------

            // 9. AppUser - Basket (Bire Bir)
            builder.Entity<AppUser>()
                .HasOne<Basket>()
                .WithOne(b => b.User)
                .HasForeignKey<Basket>(b => b.CustomerId); // Basket içinde UserId tutulur (AppUser ID'si)

            // 10. Basket - BasketItem (Bire Çok)
            builder.Entity<Basket>()
                .HasMany(b => b.BasketItems) // Basket içinde Items koleksiyonu olmalı
                .WithOne(bi => bi.Basket)
                .HasForeignKey(bi => bi.BasketId)
                .OnDelete(DeleteBehavior.Cascade); // Sepet silinirse içindeki ürünler de silinsin

            // 11. AppUser - UserComment (Bire Çok)
            builder.Entity<AppUser>()
                .HasMany(u => u.Comments) // AppUser entity'sinde Comments koleksiyonu olmalı
                .WithOne(uc => uc.Customer)
                .HasForeignKey(uc => uc.CustomerId);

            // 12. Product - UserComment (Bire Çok)
            builder.Entity<Product>()
                .HasMany(p => p.Comments)
                .WithOne(uc => uc.Product)
                .HasForeignKey(uc => uc.ProductId);

            // 15. AppUser - ProductLike (Bire Çok)
            builder.Entity<AppUser>()
                .HasMany(u => u.ProductLikes)
                .WithOne(pl => pl.User)
                .HasForeignKey(pl => pl.UserId);

            // 16. Product - ProductLike (Bire Çok)
            builder.Entity<Product>()
                .HasMany(p => p.ProductLikes)
                .WithOne(pl => pl.Product)
                .HasForeignKey(pl => pl.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // 17. ProductLike Unique Index (UserId, ProductId)
            builder.Entity<ProductLike>()
                .HasIndex(pl => new { pl.UserId, pl.ProductId })
                .IsUnique();

            // -------------------------------------------------------------
            // D. KARGO (Cargo) İlişkileri
            // -------------------------------------------------------------

            // 13. CargoCompany - CargoDetail (Bire Çok)
            builder.Entity<CargoCompany>()
                .HasMany(cc => cc.CargoDetails) // CargoCompany içinde CargoDetails koleksiyonu olmalı
                .WithOne(cd => cd.CargoCompany)      // CargoDetail içinde Company navigation property'si olmalı
                .HasForeignKey(cd => cd.CargoCompanyId);

            // 14. CargoDetail - CargoOperation (Bire Çok)
            builder.Entity<CargoDetail>()
                .HasMany(cd => cd.Operations) // CargoDetail içinde Operations koleksiyonu olmalı
                .WithOne(co => co.CargoDetail)
                .HasForeignKey(co => co.CargoDetailId);
        }

        // -----------------------------------------------------------------
        // ## 3. SaveChangesAsync
        // -----------------------------------------------------------------

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker
                .Entries<BaseEntity>();

            foreach (var data in datas)
            {
                // Mevcut kodunuzdaki gibi DateTime.UtcNow ataması.
                // switch ifadesini daha sade hale getirmek best practice'dir.
                if (data.State == EntityState.Added)
                {
                    data.Entity.CreatedDate = DateTime.UtcNow;
                }
                else if (data.State == EntityState.Modified)
                {
                    data.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}