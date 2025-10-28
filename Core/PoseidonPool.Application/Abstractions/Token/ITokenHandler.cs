using PoseidonPool.Application.DTOs;
using PoseidonPool.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        TokenDTO CreateAccessToken(int second, AppUser appUser);
        string CreateRefreshToken();
    }
}
