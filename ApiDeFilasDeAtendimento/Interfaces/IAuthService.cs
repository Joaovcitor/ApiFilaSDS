using ApiDeFilasDeAtendimento.DTOs.Auth;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IAuthService
    {
        Task<object> Login(LoginModelDto dados);
        //Task RegistrarUsuario(RegisterModelDto registerModelDto, string roleSolicitada, string adminId);
        Task Logout();
    }
}
