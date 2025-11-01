namespace PoseidonPool.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isHtml = true);
    }
}
