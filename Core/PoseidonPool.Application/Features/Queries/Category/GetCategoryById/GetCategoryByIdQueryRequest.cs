using MediatR;

namespace PoseidonPool.Application.Features.Queries.Category.GetCategoryById
{
    public class GetCategoryByIdQueryRequest : IRequest<GetCategoryByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}
