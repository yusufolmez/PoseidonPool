using MediatR;
using PoseidonPool.Application.DTOs.Catalog;

namespace PoseidonPool.Application.Features.Commands.About.UpdateAbout
{
    public class UpdateAboutCommandRequest : IRequest<UpdateAboutCommandResponse>
    {
        public AboutDTO Model { get; set; }
    }
}


