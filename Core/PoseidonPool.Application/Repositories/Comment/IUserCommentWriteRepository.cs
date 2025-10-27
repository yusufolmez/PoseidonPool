using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonPool.Application.Repositories.Comment
{
    public interface IUserCommentWriteRepository : IWriteRepository<Domain.Entities.Comment.UserComment>
    {
    }
}
