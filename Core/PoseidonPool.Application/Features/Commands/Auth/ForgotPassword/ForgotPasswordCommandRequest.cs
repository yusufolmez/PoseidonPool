using MediatR;

namespace PoseidonPool.Application.Features.Commands.Auth.ForgotPassword
{
    public class ForgotPasswordCommandRequest : IRequest<ForgotPasswordCommandResponse>
    {
        public string Email { get; set; }
    }
}


