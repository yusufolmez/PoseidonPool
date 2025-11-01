using MediatR;
using PoseidonPool.Application.Repositories.Catalog;

namespace PoseidonPool.Application.Features.Queries.ProductLike.GetProductLikes
{
    public class GetProductLikesHandler : IRequestHandler<GetProductLikesQueryRequest, GetProductLikesQueryResponse>
    {
        private readonly IProductLikeReadRepository _readRepository;

        public GetProductLikesHandler(IProductLikeReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<GetProductLikesQueryResponse> Handle(GetProductLikesQueryRequest request, CancellationToken cancellationToken)
        {
            var likeCount = await _readRepository.GetLikeCountByProductIdAsync(request.ProductId);

            return new GetProductLikesQueryResponse { LikeCount = likeCount };
        }
    }
}

