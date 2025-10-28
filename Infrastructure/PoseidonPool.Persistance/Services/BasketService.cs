using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.Repositories.Basket;
using PoseidonPool.Application.ViewModels.Basket;
using PoseidonPool.Domain.Entities.Basket;
using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Persistance.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;

        public BasketService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager,
            IBasketReadRepository basketReadRepository,
            IBasketWriteRepository basketWriteRepository,
            IBasketItemWriteRepository basketItemWriteRepository,
            IBasketItemReadRepository basketItemReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _basketReadRepository = basketReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
        }

        private async Task<Basket?> ContextBasket()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new Exception("Kullanıcı bulunamadı");

            // get user first
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            // Try to get existing basket directly from Baskets table by CustomerId.
            var existingBasket = await _basketReadRepository.Table.FirstOrDefaultAsync(b => b.CustomerId == user.Id);
            if (existingBasket != null)
                return existingBasket;

            // Not found -> create. Wrap potential unique-constraint race with try/catch and fallback read.
            var newBasket = new Basket
            {
                Id = Guid.NewGuid(),
                CustomerId = user.Id,
                User = user,
                BasketItems = new List<BasketItem>()
            };

            const int maxRetries = 3;
            int attempt = 0;
            while (true)
            {
                try
                {
                    await _basketWriteRepository.AddAsync(newBasket);
                    await _basketWriteRepository.SaveAsync();
                    return newBasket;
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                {
                    // likely a unique constraint violation from concurrent insert; try to read the existing basket a few times
                    attempt++;
                    for (int i = 0; i < 5; i++)
                    {
                        var concurrent = await _basketReadRepository.Table.FirstOrDefaultAsync(b => b.CustomerId == user.Id);
                        if (concurrent != null)
                            return concurrent;
                        await Task.Delay(50);
                    }

                    if (attempt >= maxRetries)
                        throw; // give up after retries

                    // otherwise retry the insert (fresh newGuid)
                    newBasket.Id = Guid.NewGuid();
                }
            }
        }

        public Basket? GetUserActiveBasketAsync => ContextBasket().Result;

        public async Task AddItemToBasketAsync(VM_Create_BasketItem model)
        {
            // obtain the user's basket (ContextBasket will create it if missing)
            Basket? basket = await ContextBasket();

            BasketItem? basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(model.ProductId));

            if (basketItem != null)
            {
                basketItem.Quantity++;
            }
            else
            {
                await _basketItemWriteRepository.AddAsync(new()
                {
                    BasketId = basket.Id,
                    ProductId = Guid.Parse(model.ProductId),
                    Quantity = model.Quantity
                });
            }
            await _basketItemWriteRepository.SaveAsync();
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextBasket();
            Basket result = await _basketReadRepository.Table.Include(b => b.BasketItems).ThenInclude(bi => bi.Product).FirstOrDefaultAsync(b => b.Id == basket.Id);
            return result.BasketItems.ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem model)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(model.BasketItemId);
            if (basketItem != null)
            {
                basketItem.Quantity = model.Quantity;
                _basketItemWriteRepository.Update(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }
    }
}
