using ApiDeFilasDeAtendimento.DTOs.Senhas;
using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IFilaSenhaService
    {
        Task<FilaSenha> CreateSenha(SenhaDtoCreate dados);
        Task<FilaSenha> UpdateStatusForCall(SenhaDtoUpdateStatusForCall dados);
        Task<FilaSenha> UpdateStatusForAtendimento(SenhaDtoUpdateStatusForAtendimento dados);
        Task<FilaSenha> UpdateStatusForCancel(SenhaDtoUpdateStatusForCancel dados);
        Task<FilaSenha> UpdateNameEmployee(SenhaDtoUpdateNameEmployee dados);
        Task<FilaSenha> UpdateMotivoAtendimento(SenhaDtoUpdateMotivoAtendimento dados);
        Task<List<FilaSenha>> GetAguardando();
    }
}
