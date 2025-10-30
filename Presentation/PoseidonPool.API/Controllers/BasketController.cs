using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.Basket.AddItemToBasket;
using PoseidonPool.Application.Features.Commands.Basket.RemoveBasketItem;
using PoseidonPool.Application.Features.Commands.Basket.UpdateQuantity;
using PoseidonPool.Application.Features.Queries.Basket.GetBasketItems;
using PoseidonPool.Application.Features.Queries.Basket.GetBasketSummary;
using PoseidonPool.Application.Features.Commands.Basket.ClearBasket;
using PoseidonPool.Application.Features.Queries.Basket.Guest.GetGuestItems;
using PoseidonPool.Application.Features.Commands.Basket.Guest.AddOrUpdateGuestItem;
using PoseidonPool.Application.Features.Commands.Basket.Guest.RemoveGuestItem;
using PoseidonPool.Application.Features.Commands.Basket.MergeGuestBasket;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
                var authHeader = Request.Headers["Authorization"].ToString(); // <-- breakpoint
                var isAuth = User?.Identity?.IsAuthenticated;
                var name = User?.Identity?.Name;

            var response = await _mediator.Send(new GetBasketItemsQueryRequest());
            return Ok(response);
            
        }

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var response = await _mediator.Send(new GetBasketSummaryQueryRequest());
            return Ok(response);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddItemToBasketCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("items/{basketItemId}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] string basketItemId, [FromBody] UpdateQuantityCommandRequest request)
        {
            request.BasketItemId = basketItemId;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("items/{basketItemId}")]
        public async Task<IActionResult> RemoveItem([FromRoute] string basketItemId)
        {
            var response = await _mediator.Send(new RemoveBasketItemCommandRequest { BasketItemId = basketItemId });
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            var response = await _mediator.Send(new ClearBasketCommandRequest());
            return Ok(response);
        }

        // Guest basket endpoints (in-memory store, can be swapped to Redis)
        [HttpGet("guest/items")]
        public async Task<IActionResult> GetGuestItems([FromQuery] string guestId)
        {
            var response = await _mediator.Send(new GetGuestItemsQueryRequest { GuestId = guestId });
            return Ok(response);
        }

        [HttpPost("guest/items")]
        public async Task<IActionResult> AddOrUpdateGuestItem([FromBody] AddOrUpdateGuestItemCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("guest/items/{productId}")]
        public async Task<IActionResult> RemoveGuestItem([FromRoute] Guid productId, [FromQuery] string guestId)
        {
            var response = await _mediator.Send(new RemoveGuestItemCommandRequest { GuestId = guestId, ProductId = productId });
            return Ok(response);
        }

        [HttpPost("merge")]
        public async Task<IActionResult> Merge([FromBody] MergeGuestBasketCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
