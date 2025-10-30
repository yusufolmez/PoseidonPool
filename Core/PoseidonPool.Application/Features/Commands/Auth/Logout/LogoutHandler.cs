using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Auth.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommandRequest, LogoutCommandResponse>
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _http;
        public LogoutHandler(IAuthService authService, IHttpContextAccessor http)
        {
            _authService = authService;
            _http = http;
        }

        public async Task<LogoutCommandResponse> Handle(LogoutCommandRequest request, CancellationToken cancellationToken)
        {
            var userName = _http.HttpContext?.User?.Identity?.Name;
            var ok = await _authService.LogoutAsync(userName);
            return new LogoutCommandResponse { Success = ok };
        }
    }
}


