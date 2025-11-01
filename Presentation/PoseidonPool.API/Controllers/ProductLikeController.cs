using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.ProductLike.Add;
using PoseidonPool.Application.Features.Commands.ProductLike.Remove;
using PoseidonPool.Application.Features.Queries.ProductLike.CheckProductLike;
using PoseidonPool.Application.Features.Queries.ProductLike.GetProductLikes;
using PoseidonPool.Application.Features.Queries.ProductLike.GetUserLikes;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductLikeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductLikeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductLikeCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> Remove([FromRoute] Guid productId)
        {
            var request = new RemoveProductLikeCommandRequest { ProductId = productId };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserLikes()
        {
            var response = await _mediator.Send(new GetUserLikesQueryRequest());
            return Ok(response);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductLikes([FromRoute] Guid productId)
        {
            var request = new GetProductLikesQueryRequest { ProductId = productId };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("check/{productId}")]
        public async Task<IActionResult> CheckProductLike([FromRoute] Guid productId)
        {
            var request = new CheckProductLikeQueryRequest { ProductId = productId };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}

