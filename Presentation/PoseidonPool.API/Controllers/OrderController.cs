using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.Order.CreateOrder;
using PoseidonPool.Application.Features.Commands.Order.CompleteOrder;
using PoseidonPool.Application.Features.Queries.Order.GetAllOrders;
using PoseidonPool.Application.Features.Queries.Order.GetOrderById;
using Microsoft.AspNetCore.Authorization;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllOrdersQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await _mediator.Send(new GetOrderByIdQueryRequest { Id = id });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("complete/{id}")]
        public async Task<IActionResult> Complete([FromRoute] string id)
        {
            var response = await _mediator.Send(new CompleteOrderCommandRequest { Id = id });
            return Ok(response);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Queries.Order.GetMyOrders.GetMyOrdersQueryRequest());
            return Ok(response);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus([FromRoute] string status)
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Queries.Order.GetByStatus.GetOrdersByStatusQueryRequest { Status = status });
            return Ok(response);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] string id)
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Commands.Order.CancelOrder.CancelOrderCommandRequest { Id = id });
            return Ok(response);
        }

        [HttpPut("{id}/ship")]
        public async Task<IActionResult> Ship([FromRoute] string id)
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Commands.Order.ShipOrder.ShipOrderCommandRequest { Id = id });
            return Ok(response);
        }

        [HttpPut("{id}/deliver")]
        public async Task<IActionResult> Deliver([FromRoute] string id)
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Commands.Order.DeliverOrder.DeliverOrderCommandRequest { Id = id });
            return Ok(response);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Queries.Order.GetOrderDetails.GetOrderDetailsQueryRequest { Id = id });
            return Ok(response);
        }
    }
}
