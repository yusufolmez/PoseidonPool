using PoseidonPool.Application.Repositories.Comment;
using PoseidonPool.Domain.Entities.Comment;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Comment
{
    public class UserCommentReadRepository : ReadRepository<UserComment>, IUserCommentReadRepository
    {
        public UserCommentReadRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
