using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ApiDeFilasDeAtendimento.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IHostEnvironment _env;

        public EmailService(IOptions<EmailSettings> settings, IHostEnvironment env)
        {
            _settings = settings.Value;
            _env = env;
        }
        public async Task SendPasswordResetAsync(string email, string token)
        {
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            string resetLink = $"{_settings.BaseUrl}/recuperar-senha?token={encodedToken}&email={email}";

            string path = Path.Combine(_env.ContentRootPath, "Templates", "PasswordReset.html");
            string template = await File.ReadAllTextAsync(path);

            string body = template.Replace("{link}", resetLink);

            await EnviarNoGmailAsync(email, "Recuperação de Senha", body);
        }

        public async Task SendWelcomeAsync(string nomeCompleto, string email)
        {
            string path = Path.Combine(_env.ContentRootPath, "Templates", "Welcome.html");
            string template = await File.ReadAllTextAsync(path);
            string body = template.Replace("{nome}", nomeCompleto);
            await EnviarNoGmailAsync(email, "Bem vindo ao sistema FilaFlow!", body);
        }

        private async Task EnviarNoGmailAsync(string para, string assunto, string html)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(para));
            email.Subject = assunto;

            var body = new BodyBuilder { HtmlBody = html };
            email.Body = body.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
