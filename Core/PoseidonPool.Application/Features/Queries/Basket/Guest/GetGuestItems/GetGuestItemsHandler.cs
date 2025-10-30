using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Basket.Guest.GetGuestItems
{
    public class GetGuestItemsHandler : IRequestHandler<GetGuestItemsQueryRequest, GetGuestItemsQueryResponse>
    {
        private readonly IGuestBasketStore _store;

        public GetGuestItemsHandler(IGuestBasketStore store)
        {
            _store = store;
        }

        public async Task<GetGuestItemsQueryResponse> Handle(GetGuestItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basket = await _store.GetAsync(request.GuestId);
            return new GetGuestItemsQueryResponse { Items = basket.Items };
        }
    }
}


