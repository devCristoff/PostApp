using PostApp.Core.Application.DTOs.Email;

namespace PostApp.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
