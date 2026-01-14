using ApiDeFilasDeAtendimento.DTOs.Auth;
using ApiDeFilasDeAtendimento.DTOs.Managements;
using ApiDeFilasDeAtendimento.DTOs.Pagination;
using Microsoft.AspNetCore.Identity;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IManagementService
    {
        Task<PagedResult<UserDtoResponse>> ListarMeusUsuariosAsync(string donoId, int page, int pageSize);
        Task<UserDtoResponse> ObterPorIdAsync(string id, string donoId);
        Task<IdentityResult> CriarUsuarioAsync(RegisterModelDto dados,  string donoId, string role);
        Task<IdentityResult> AtualizarAsync(string id, UserDtoUpdate dados, string donoId);
    }
}
