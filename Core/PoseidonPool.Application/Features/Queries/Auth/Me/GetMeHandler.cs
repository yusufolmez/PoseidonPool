using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.Auth.Me
{
    public class GetMeHandler : IRequestHandler<GetMeQueryRequest, GetMeQueryResponse>
    {
        private readonly IAuthService _auth;
        private readonly IHttpContextAccessor _http;
        public GetMeHandler(IAuthService auth, IHttpContextAccessor http)
        {
            _auth = auth;
            _http = http;
        }

        public async Task<GetMeQueryResponse> Handle(GetMeQueryRequest request, CancellationToken cancellationToken)
        {
            var name = _http.HttpContext?.User?.Identity?.Name;
            var me = await _auth.GetMeAsync(name);
            return new GetMeQueryResponse { User = me };
        }
    }
}


