using PoseidonPool.Application.Abstractions.Services;
using PoseidonPool.Application.Abstractions.Token;
using PoseidonPool.Application.DTOs;
using PoseidonPool.Application.Exceptions;
using PoseidonPool.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PoseidonPool.Persistance.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly IUserService _userService;

        public AuthService(HttpClient httpClient, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
        }
        async Task<TokenDTO> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info);

                TokenDTO token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration.ToUniversalTime(), 15);
                return token;
            }
            throw new Exception("Invalid external authentication.");
        }

        public async Task<TokenDTO> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);
        }

        public async Task<TokenDTO> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                TokenDTO token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration.ToUniversalTime(), 15);
                return token;
            }
            throw new AuthenticationErrorException();
        }

        public async Task<TokenDTO> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                TokenDTO token = _tokenHandler.CreateAccessToken(15, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration.ToUniversalTime(), 300);
                return token;
            }
            else
                throw new NotFoundUserException();
        }

        public async Task<bool> LogoutAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return false;
            user.RefreshToken = null;
            user.RefreshTokenEndDate = null;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<Application.DTOs.User.MeDTO> GetMeAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return new Application.DTOs.User.MeDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NameSurname = user.NameSurname,
                Roles = roles?.ToArray() ?? System.Array.Empty<string>()
            };
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // TODO: e-posta sağlayıcısı aracılığıyla jeton gönder. Şimdilik, harici olarak günlüğe kaydet veya sakla
            // Göndermeden kasıtlı olarak true döndür.
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task<bool> ResetPasswordAsync(string email, string resetToken, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return false;
            user.RefreshToken = null;
            user.RefreshTokenEndDate = null;
            await _userManager.UpdateAsync(user);
            return true;
        }
    }
}
