using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.Mail.TestMail;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailTestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MailTestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> TestMail([FromBody] TestMailCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}

