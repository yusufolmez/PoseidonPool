using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.About.UpdateAbout
{
    public class UpdateAboutHandler : IRequestHandler<UpdateAboutCommandRequest, UpdateAboutCommandResponse>
    {
        private readonly IAboutService _service;
        public UpdateAboutHandler(IAboutService service)
        {
            _service = service;
        }

        public async Task<UpdateAboutCommandResponse> Handle(UpdateAboutCommandRequest request, CancellationToken cancellationToken)
        {
            var dto = await _service.UpdateAsync(request.Model);
            return new UpdateAboutCommandResponse { About = dto };
        }
    }
}


