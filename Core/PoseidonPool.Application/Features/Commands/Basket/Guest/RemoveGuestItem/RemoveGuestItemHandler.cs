using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Basket.Guest.RemoveGuestItem
{
    public class RemoveGuestItemHandler : IRequestHandler<RemoveGuestItemCommandRequest, RemoveGuestItemCommandResponse>
    {
        private readonly IGuestBasketStore _store;

        public RemoveGuestItemHandler(IGuestBasketStore store)
        {
            _store = store;
        }

        public async Task<RemoveGuestItemCommandResponse> Handle(RemoveGuestItemCommandRequest request, CancellationToken cancellationToken)
        {
            await _store.RemoveAsync(request.GuestId, request.ProductId);
            return new RemoveGuestItemCommandResponse { Success = true };
        }
    }
}


