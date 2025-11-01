using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.DTOs.User;

namespace PoseidonPool.Application.Features.Commands.Auth.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest, RegisterUserCommandResponse>
    {
        private readonly IUserService _userService;

        public RegisterUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {
            var dto = new CreateUserRequestDTO
            {
                NameSurname = request.NameSurname,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userService.CreateAsync(dto);
            return new RegisterUserCommandResponse { Result = result };
        }
    }
}
