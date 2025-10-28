using MediatR;

namespace PoseidonPool.Application.Features.Commands.Auth.LoginUser
{
    public class LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
        public int AccessTokenLifeTime { get; set; } = 900; // seconds
    }
}
