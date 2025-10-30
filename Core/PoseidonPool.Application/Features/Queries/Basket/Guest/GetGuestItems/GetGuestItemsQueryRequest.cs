using MediatR;
using PoseidonPool.Application.DTOs.Basket;
using System.Collections.Generic;

namespace PoseidonPool.Application.Features.Queries.Basket.Guest.GetGuestItems
{
    public class GetGuestItemsQueryRequest : IRequest<GetGuestItemsQueryResponse>
    {
        public string GuestId { get; set; }
    }
}


