using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.Basket;

namespace PoseidonPool.Infrastructure.Services.Storage
{
    // NOTE: This is an in-memory placeholder. Swap with a Redis-backed implementation later.
    public class InMemoryGuestBasketStore : IGuestBasketStore
    {
        private static readonly ConcurrentDictionary<string, GuestBasketDTO> Store = new();

        public Task<GuestBasketDTO> GetAsync(string guestId)
        {
            if (string.IsNullOrWhiteSpace(guestId)) throw new ArgumentException("guestId is required", nameof(guestId));
            var basket = Store.GetOrAdd(guestId, gid => new GuestBasketDTO { GuestId = gid });
            return Task.FromResult(basket);
        }

        public async Task AddOrUpdateAsync(string guestId, Guid productId, int quantity)
        {
            if (quantity <= 0) quantity = 1;
            var basket = await GetAsync(guestId);
            var existing = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing == null)
            {
                basket.Items.Add(new GuestBasketItemDTO { ProductId = productId, Quantity = quantity, AddedAt = DateTime.UtcNow });
            }
            else
            {
                existing.Quantity += quantity;
            }
        }

        public async Task RemoveAsync(string guestId, Guid productId)
        {
            var basket = await GetAsync(guestId);
            basket.Items = basket.Items.Where(i => i.ProductId != productId).ToList();
        }

        public Task ClearAsync(string guestId)
        {
            Store.TryRemove(guestId, out _);
            return Task.CompletedTask;
        }
    }
}


