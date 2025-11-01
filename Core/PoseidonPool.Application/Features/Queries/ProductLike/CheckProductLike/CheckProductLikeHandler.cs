using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Application.Features.Queries.ProductLike.CheckProductLike
{
    public class CheckProductLikeHandler : IRequestHandler<CheckProductLikeQueryRequest, CheckProductLikeQueryResponse>
    {
        private readonly IProductLikeReadRepository _readRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public CheckProductLikeHandler(
            IProductLikeReadRepository readRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager)
        {
            _readRepository = readRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<CheckProductLikeQueryResponse> Handle(CheckProductLikeQueryRequest request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return new CheckProductLikeQueryResponse { IsLiked = false };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return new CheckProductLikeQueryResponse { IsLiked = false };
            }

            var isLiked = await _readRepository.IsLikedByUserAsync(user.Id, request.ProductId);

            return new CheckProductLikeQueryResponse { IsLiked = isLiked };
        }
    }
}

