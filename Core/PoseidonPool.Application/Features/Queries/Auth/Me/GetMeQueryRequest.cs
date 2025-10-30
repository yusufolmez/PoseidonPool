using MediatR;
using PoseidonPool.Application.DTOs.User;

namespace PoseidonPool.Application.Features.Queries.Auth.Me
{
    public class GetMeQueryRequest : IRequest<GetMeQueryResponse>
    {
    }
}


