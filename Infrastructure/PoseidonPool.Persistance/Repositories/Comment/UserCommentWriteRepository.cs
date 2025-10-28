using PoseidonPool.Application.Repositories.Comment;
using PoseidonPool.Domain.Entities.Comment;
using PoseidonPool.Persistance.Contexts;

namespace PoseidonPool.Persistance.Repositories.Comment
{
    public class UserCommentWriteRepository : WriteRepository<UserComment>, IUserCommentWriteRepository
    {
        public UserCommentWriteRepository(PoseidonPoolDBContext context) : base(context)
        {
        }
    }
}
