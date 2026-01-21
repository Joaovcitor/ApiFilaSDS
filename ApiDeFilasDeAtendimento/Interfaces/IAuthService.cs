using ApiDeFilasDeAtendimento.DTOs.Auth;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IAuthService
    {
        Task<object> Login(LoginModelDto dados);
        Task Logout();
        Task RequestPasswordReset(string email);
        Task ResetPassword(ResetPasswordDto dados);
    }
}
