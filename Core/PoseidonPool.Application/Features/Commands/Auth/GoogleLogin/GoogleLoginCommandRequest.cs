using MediatR;

namespace PoseidonPool.Application.Features.Commands.Auth.GoogleLogin
{
    public class GoogleLoginCommandRequest : IRequest<GoogleLoginCommandResponse>
    {
        public string IdToken { get; set; }
        public int AccessTokenLifeTime { get; set; } = 900; // seconds
    }
}
