using System.ComponentModel.DataAnnotations;
using MediatR;

namespace PoseidonPool.Application.Features.Commands.Mail.TestMail
{
    public class TestMailCommandRequest : IRequest<TestMailCommandResponse>
    {
        [Required(ErrorMessage = "To is required")]
        [EmailAddress(ErrorMessage = "To must be a valid email address")]
        public string To { get; set; }

        public string Subject { get; set; } = "Test Mail from PoseidonPool";

        public string Body { get; set; } = "This is a test email from PoseidonPool SMTP service.";
    }
}

