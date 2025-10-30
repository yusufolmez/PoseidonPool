using MediatR;

namespace PoseidonPool.Application.Features.Commands.Basket.MergeGuestBasket
{
    public class MergeGuestBasketCommandRequest : IRequest<MergeGuestBasketCommandResponse>
    {
        public string GuestId { get; set; }
    }
}


