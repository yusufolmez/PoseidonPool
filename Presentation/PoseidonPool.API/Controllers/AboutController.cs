using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AboutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new PoseidonPool.Application.Features.Queries.About.GetAbout.GetAboutQueryRequest());
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] PoseidonPool.Application.Features.Commands.About.UpdateAbout.UpdateAboutCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}


