using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.ViewModels.Basket;

namespace PoseidonPool.Application.Features.Commands.Basket.UpdateQuantity
{
    public class UpdateQuantityHandler : IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>
    {
        private readonly IBasketService _basketService;

        public UpdateQuantityHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<UpdateQuantityCommandResponse> Handle(UpdateQuantityCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.UpdateQuantityAsync(new VM_Update_BasketItem
            {
                BasketItemId = request.BasketItemId,
                Quantity = request.Quantity
            });

            return new UpdateQuantityCommandResponse
            {
                Success = true,
                Message = "Basket item quantity updated."
            };
        }
    }
}
