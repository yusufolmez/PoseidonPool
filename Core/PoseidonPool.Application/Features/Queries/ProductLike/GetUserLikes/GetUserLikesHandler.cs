using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.DTOs.Catalog;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Identity;

namespace PoseidonPool.Application.Features.Queries.ProductLike.GetUserLikes
{
    public class GetUserLikesHandler : IRequestHandler<GetUserLikesQueryRequest, GetUserLikesQueryResponse>
    {
        private readonly IProductLikeReadRepository _readRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public GetUserLikesHandler(
            IProductLikeReadRepository readRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager)
        {
            _readRepository = readRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<GetUserLikesQueryResponse> Handle(GetUserLikesQueryRequest request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return new GetUserLikesQueryResponse { Likes = new List<ProductLikeDTO>() };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return new GetUserLikesQueryResponse { Likes = new List<ProductLikeDTO>() };
            }

            var likes = await _readRepository.GetLikesByUserIdAsync(user.Id);

            var likeDTOs = likes.Select(like => new ProductLikeDTO
            {
                Id = like.Id,
                UserId = like.UserId,
                ProductId = like.ProductId,
                CreatedDate = like.CreatedDate,
                Product = like.Product != null ? new ProductDTO
                {
                    Id = like.Product.Id,
                    ProductName = like.Product.ProductName,
                    ProductPrice = like.Product.ProductPrice,
                    CategoryId = like.Product.CategoryId,
                    BrandId = like.Product.BrandId,
                    CreatedDate = like.Product.CreatedDate,
                    UpdatedDate = like.Product.UpdatedDate
                } : null
            }).ToList();

            return new GetUserLikesQueryResponse { Likes = likeDTOs };
        }
    }
}

