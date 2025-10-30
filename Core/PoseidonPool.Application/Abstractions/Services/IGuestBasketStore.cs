using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoseidonPool.Application.DTOs.Basket;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IGuestBasketStore
    {
        Task<GuestBasketDTO> GetAsync(string guestId);
        Task AddOrUpdateAsync(string guestId, Guid productId, int quantity);
        Task RemoveAsync(string guestId, Guid productId);
        Task ClearAsync(string guestId);
    }
}


