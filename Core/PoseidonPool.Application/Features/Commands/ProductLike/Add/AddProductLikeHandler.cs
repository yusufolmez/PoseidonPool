using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoseidonPool.Application.Repositories.Catalog;
using PoseidonPool.Domain.Entities.Catalog;
using PoseidonPool.Domain.Entities.Identity;
using ProductLikeEntity = PoseidonPool.Domain.Entities.Catalog.ProductLike;

namespace PoseidonPool.Application.Features.Commands.ProductLike.Add
{
    public class AddProductLikeHandler : IRequestHandler<AddProductLikeCommandRequest, AddProductLikeCommandResponse>
    {
        private readonly IProductLikeReadRepository _readRepository;
        private readonly IProductLikeWriteRepository _writeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public AddProductLikeHandler(
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

        public async Task<AddProductLikeCommandResponse> Handle(AddProductLikeCommandRequest request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return new AddProductLikeCommandResponse
                {
                    Success = false,
                    Message = "User not authenticated"
                };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return new AddProductLikeCommandResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var existingLike = await _readRepository.GetByUserAndProductAsync(user.Id, request.ProductId);
            if (existingLike != null)
            {
                return new AddProductLikeCommandResponse
                {
                    Success = false,
                    Message = "Product already liked"
                };
            }

            var like = new ProductLikeEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ProductId = request.ProductId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _writeRepository.AddAsync(like);
            await _writeRepository.SaveAsync();

            return new AddProductLikeCommandResponse
            {
                Success = true,
                Message = "Product liked successfully"
            };
        }
    }
}

