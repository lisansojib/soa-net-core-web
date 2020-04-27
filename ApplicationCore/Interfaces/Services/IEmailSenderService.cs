using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
