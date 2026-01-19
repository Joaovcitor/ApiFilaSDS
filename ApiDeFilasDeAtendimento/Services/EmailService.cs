using ApiDeFilasDeAtendimento.Interfaces;

namespace ApiDeFilasDeAtendimento.Services
{
    public class EmailService : IEmailService
    {
        public Task ResetPasswordAsync(string email, string resetLink)
        {
            throw new NotImplementedException();
        }

        public Task SendWelcomeAsync(string nomeCompleto, string email)
        {
            throw new NotImplementedException();
        }
    }
}
