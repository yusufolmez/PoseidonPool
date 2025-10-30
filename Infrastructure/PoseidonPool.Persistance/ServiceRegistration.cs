using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.Repositories.Basket;
using PoseidonPool.Application.Repositories.Order;
using PoseidonPool.Domain.Entities.Identity;
using PoseidonPool.Persistance.Contexts;
using PoseidonPool.Persistance.Repositories.Basket;
using PoseidonPool.Persistance.Repositories.Order;
using PoseidonPool.Persistance.Services;
using PoseidonPool.Application.Repositories.Comment;
using PoseidonPool.Persistance.Repositories.Comment;

namespace PoseidonPool.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<PoseidonPoolDBContext>(options => options.UseNpgsql(Configuration.ConncetString));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<PoseidonPoolDBContext>();

            // Repositories
            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();
            services.AddScoped<IOrderingReadRepository, OrderingReadRepository>();
            services.AddScoped<IOrderingWriteRepository, OrderingWriteRepository>();
            // Catalog repositories
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IProductReadRepository, PoseidonPool.Persistance.Repositories.Catalog.ProductReadRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IProductWriteRepository, PoseidonPool.Persistance.Repositories.Catalog.ProductWriteRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IProductImageReadRepository, PoseidonPool.Persistance.Repositories.Catalog.ProductImageReadRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IProductImageWriteRepository, PoseidonPool.Persistance.Repositories.Catalog.ProductImageWriteRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IProductDetailReadRepository, PoseidonPool.Persistance.Repositories.Catalog.ProductDetailReadRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IProductDetailWriteRepository, PoseidonPool.Persistance.Repositories.Catalog.ProductDetailWriteRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IBrandReadRepository, PoseidonPool.Persistance.Repositories.Catalog.BrandReadRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.IBrandWriteRepository, PoseidonPool.Persistance.Repositories.Catalog.BrandWriteRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.ICategoryReadRepository, PoseidonPool.Persistance.Repositories.Catalog.CategoryReadRepository>();
            services.AddScoped<PoseidonPool.Application.Repositories.Catalog.ICategoryWriteRepository, PoseidonPool.Persistance.Repositories.Catalog.CategoryWriteRepository>();

            // Comment repositories
            services.AddScoped<IUserCommentReadRepository, UserCommentReadRepository>();
            services.AddScoped<IUserCommentWriteRepository, UserCommentWriteRepository>();

            // Services
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            // Product service
            services.AddScoped<PoseidonPool.Application.Abstractions.Services.IProductService, PoseidonPool.Persistance.Services.ProductService>();
            services.AddScoped<PoseidonPool.Application.Abstractions.Services.IBrandService, PoseidonPool.Persistance.Services.BrandService>();
            services.AddScoped<PoseidonPool.Application.Abstractions.Services.ICategoryService, PoseidonPool.Persistance.Services.CategoryService>();
        }
    }
}
