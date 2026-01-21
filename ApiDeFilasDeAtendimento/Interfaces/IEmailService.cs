namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeAsync(string nomeCompleto, string email);
        Task SendPasswordResetAsync(string email, string token);
    }
}
