using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Basket.Guest.AddOrUpdateGuestItem
{
    public class AddOrUpdateGuestItemHandler : IRequestHandler<AddOrUpdateGuestItemCommandRequest, AddOrUpdateGuestItemCommandResponse>
    {
        private readonly IGuestBasketStore _store;

        public AddOrUpdateGuestItemHandler(IGuestBasketStore store)
        {
            _store = store;
        }

        public async Task<AddOrUpdateGuestItemCommandResponse> Handle(AddOrUpdateGuestItemCommandRequest request, CancellationToken cancellationToken)
        {
            await _store.AddOrUpdateAsync(request.GuestId, request.ProductId, request.Quantity);
            return new AddOrUpdateGuestItemCommandResponse { Success = true };
        }
    }
}


