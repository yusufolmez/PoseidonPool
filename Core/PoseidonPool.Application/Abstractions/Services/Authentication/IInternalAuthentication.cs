using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Application.Abstractions.Services.Authentication
{
    public interface IInternalAuthentication
    {
        Task<DTOs.TokenDTO> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime);
        Task<DTOs.TokenDTO> RefreshTokenLoginAsync(string refreshToken);
    }
}
