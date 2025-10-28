using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.Basket.AddItemToBasket;
using PoseidonPool.Application.Features.Commands.Basket.RemoveBasketItem;
using PoseidonPool.Application.Features.Commands.Basket.UpdateQuantity;
using PoseidonPool.Application.Features.Queries.Basket.GetBasketItems;

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
    }
}
