using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Auth.RevokeRefresh
{
    public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommandRequest, RevokeRefreshTokenCommandResponse>
    {
        private readonly IAuthService _auth;
        private readonly IHttpContextAccessor _http;
        public RevokeRefreshTokenHandler(IAuthService auth, IHttpContextAccessor http)
        {
            _auth = auth;
            _http = http;
        }

        public async Task<RevokeRefreshTokenCommandResponse> Handle(RevokeRefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            var name = _http.HttpContext?.User?.Identity?.Name;
            var ok = await _auth.RevokeRefreshTokenAsync(name);
            return new RevokeRefreshTokenCommandResponse { Success = ok };
        }
    }
}


