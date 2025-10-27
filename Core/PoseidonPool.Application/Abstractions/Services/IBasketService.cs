using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task GetBasketItemsAsync();
        public Task AddItemToBasketAsync();
        public Task UpdateQuantityAsync();
        public Task RemoveBasketItemAsync(string basketItemId);
        public Task GetUserActiveBasketAsync { get; } 
    }
}
