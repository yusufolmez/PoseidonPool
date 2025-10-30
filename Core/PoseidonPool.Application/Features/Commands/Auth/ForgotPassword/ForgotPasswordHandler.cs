using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Auth.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommandRequest, ForgotPasswordCommandResponse>
    {
        private readonly IAuthService _auth;
        public ForgotPasswordHandler(IAuthService auth)
        {
            _auth = auth;
        }

        public async Task<ForgotPasswordCommandResponse> Handle(ForgotPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            var ok = await _auth.ForgotPasswordAsync(request.Email);
            return new ForgotPasswordCommandResponse { Success = ok };
        }
    }
}


