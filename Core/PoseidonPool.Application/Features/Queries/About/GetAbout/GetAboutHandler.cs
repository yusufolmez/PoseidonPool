using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Queries.About.GetAbout
{
    public class GetAboutHandler : IRequestHandler<GetAboutQueryRequest, GetAboutQueryResponse>
    {
        private readonly IAboutService _service;
        public GetAboutHandler(IAboutService service)
        {
            _service = service;
        }

        public async Task<GetAboutQueryResponse> Handle(GetAboutQueryRequest request, CancellationToken cancellationToken)
        {
            var dto = await _service.GetAsync();
            return new GetAboutQueryResponse { About = dto };
        }
    }
}


