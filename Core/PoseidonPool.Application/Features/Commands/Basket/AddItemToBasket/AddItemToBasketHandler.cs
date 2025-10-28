using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.ViewModels.Basket;

namespace PoseidonPool.Application.Features.Commands.Basket.AddItemToBasket
{
    public class AddItemToBasketHandler : IRequestHandler<AddItemToBasketCommandRequest, AddItemToBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public AddItemToBasketHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<AddItemToBasketCommandResponse> Handle(AddItemToBasketCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.AddItemToBasketAsync(new VM_Create_BasketItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });

            return new AddItemToBasketCommandResponse
            {
                Success = true,
                Message = "Item added to basket."
            };
        }
    }
}
