using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoseidonPool.Application.Features.Commands.Auth.GoogleLogin;
using PoseidonPool.Application.Features.Commands.Auth.LoginUser;
using PoseidonPool.Application.Features.Commands.Auth.RefreshTokenLogin;
using PoseidonPool.Application.Features.Commands.Auth.RegisterUser;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenLoginCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("google")] 
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
