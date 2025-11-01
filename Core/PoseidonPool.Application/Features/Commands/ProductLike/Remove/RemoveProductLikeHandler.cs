using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Application.Features.Commands.ProductLike.Remove
{
    public class RemoveProductLikeHandler : IRequestHandler<RemoveProductLikeCommandRequest, RemoveProductLikeCommandResponse>
    {
        private readonly IProductLikeReadRepository _readRepository;
        private readonly IProductLikeWriteRepository _writeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public RemoveProductLikeHandler(
            IProductLikeReadRepository readRepository,
            IProductLikeWriteRepository writeRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<RemoveProductLikeCommandResponse> Handle(RemoveProductLikeCommandRequest request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return new RemoveProductLikeCommandResponse
                {
                    Success = false,
                    Message = "User not authenticated"
                };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return new RemoveProductLikeCommandResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var existingLike = await _readRepository.GetByUserAndProductAsync(user.Id, request.ProductId);
            if (existingLike == null)
            {
                return new RemoveProductLikeCommandResponse
                {
                    Success = false,
                    Message = "Like not found"
                };
            }

            _writeRepository.Remove(existingLike);
            await _writeRepository.SaveAsync();

            return new RemoveProductLikeCommandResponse
            {
                Success = true,
                Message = "Like removed successfully"
            };
        }
    }
}

