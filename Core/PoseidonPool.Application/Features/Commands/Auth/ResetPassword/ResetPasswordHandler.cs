using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Auth.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommandRequest, ResetPasswordCommandResponse>
    {
        private readonly IAuthService _auth;
        public ResetPasswordHandler(IAuthService auth)
        {
            _auth = auth;
        }

        public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            var ok = await _auth.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
            return new ResetPasswordCommandResponse { Success = ok };
        }
    }
}


