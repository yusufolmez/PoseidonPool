using MediatR;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Application.Features.Commands.Mail.TestMail
{
    public class TestMailHandler : IRequestHandler<TestMailCommandRequest, TestMailCommandResponse>
    {
        private readonly IMailService _mailService;

        public TestMailHandler(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task<TestMailCommandResponse> Handle(TestMailCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _mailService.SendMailAsync(
                    request.To,
                    request.Subject,
                    request.Body,
                    true);

                return new TestMailCommandResponse
                {
                    Success = true,
                    Message = "Mail sent successfully"
                };
            }
            catch (Exception ex)
            {
                return new TestMailCommandResponse
                {
                    Success = false,
                    Message = $"Failed to send mail: {ex.Message}"
                };
            }
        }
    }
}

