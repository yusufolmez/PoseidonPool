using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Basket.GetBasketSummary
{
    public class GetBasketSummaryHandler : IRequestHandler<GetBasketSummaryQueryRequest, GetBasketSummaryQueryResponse>
    {
        private readonly IBasketService _basketService;

        public GetBasketSummaryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<GetBasketSummaryQueryResponse> Handle(GetBasketSummaryQueryRequest request, CancellationToken cancellationToken)
        {
            var (count, total) = await _basketService.GetSummaryAsync();
            return new GetBasketSummaryQueryResponse { ItemCount = count, TotalAmount = total };
        }
    }
}


