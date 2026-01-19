namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeAsync(string nomeCompleto, string email);
        Task ResetPasswordAsync(string email, string resetLink);
    }
}
