using ApplicationCore.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
