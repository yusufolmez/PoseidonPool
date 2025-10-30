using System;
using MediatR;

namespace PoseidonPool.Application.Features.Commands.Basket.Guest.RemoveGuestItem
{
    public class RemoveGuestItemCommandRequest : IRequest<RemoveGuestItemCommandResponse>
    {
        public string GuestId { get; set; }
        public Guid ProductId { get; set; }
    }
}


