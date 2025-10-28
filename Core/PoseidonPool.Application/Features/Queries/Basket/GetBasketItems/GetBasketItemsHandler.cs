using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsHandler : IRequestHandler<GetBasketItemsQueryRequest, GetBasketItemsQueryResponse>
    {
        private readonly IBasketService _basketService;

        public GetBasketItemsHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<GetBasketItemsQueryResponse> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var items = await _basketService.GetBasketItemsAsync();
            return new GetBasketItemsQueryResponse
            {
                Items = items
            };
        }
    }
}
