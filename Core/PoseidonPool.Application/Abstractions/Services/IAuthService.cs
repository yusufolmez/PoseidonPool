using PoseidonPool.Application.Abstractions.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
        Task<bool> LogoutAsync(string userName);
        Task<DTOs.User.MeDTO> GetMeAsync(string userName);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string resetToken, string newPassword);
        Task<bool> RevokeRefreshTokenAsync(string userName);
    }
}
