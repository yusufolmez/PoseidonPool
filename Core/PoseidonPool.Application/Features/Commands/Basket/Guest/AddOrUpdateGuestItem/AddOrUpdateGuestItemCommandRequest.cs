using System;
using MediatR;

namespace PoseidonPool.Application.Features.Commands.Basket.Guest.AddOrUpdateGuestItem
{
    public class AddOrUpdateGuestItemCommandRequest : IRequest<AddOrUpdateGuestItemCommandResponse>
    {
        public string GuestId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}


