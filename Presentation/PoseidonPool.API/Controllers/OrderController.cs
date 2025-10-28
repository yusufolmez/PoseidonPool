using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.Order.CreateOrder;
using PoseidonPool.Application.Features.Commands.Order.CompleteOrder;
using PoseidonPool.Application.Features.Queries.Order.GetAllOrders;
using PoseidonPool.Application.Features.Queries.Order.GetOrderById;

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
    }
}
