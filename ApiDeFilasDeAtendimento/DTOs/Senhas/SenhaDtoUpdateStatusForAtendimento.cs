using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.Senhas
{
    public class SenhaDtoUpdateStatusForAtendimento
    {
        public Guid Id { get; set; }
        public StatusSenha StatusSenha { get; set; } = StatusSenha.EM_ATENDIMENTO;
    }
}
