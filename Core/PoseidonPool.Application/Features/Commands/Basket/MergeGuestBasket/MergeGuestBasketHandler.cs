using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.ViewModels.Basket;

namespace PoseidonPool.Application.Features.Commands.Basket.MergeGuestBasket
{
    public class MergeGuestBasketHandler : IRequestHandler<MergeGuestBasketCommandRequest, MergeGuestBasketCommandResponse>
    {
        private readonly IGuestBasketStore _store;
        private readonly IBasketService _basketService;

        public MergeGuestBasketHandler(IGuestBasketStore store, IBasketService basketService)
        {
            _store = store;
            _basketService = basketService;
        }

        public async Task<MergeGuestBasketCommandResponse> Handle(MergeGuestBasketCommandRequest request, CancellationToken cancellationToken)
        {
            var basket = await _store.GetAsync(request.GuestId);
            var items = basket?.Items ?? new System.Collections.Generic.List<Application.DTOs.Basket.GuestBasketItemDTO>();
            var mergedCount = 0;
            foreach (var it in items)
            {
                await _basketService.AddItemToBasketAsync(new VM_Create_BasketItem { ProductId = it.ProductId.ToString(), Quantity = it.Quantity });
                mergedCount++;
            }
            await _store.ClearAsync(request.GuestId);
            return new MergeGuestBasketCommandResponse { Success = true, MergedItems = mergedCount };
        }
    }
}


