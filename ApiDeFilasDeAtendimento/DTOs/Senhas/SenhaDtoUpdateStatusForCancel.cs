using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.Senhas
{
    public class SenhaDtoUpdateStatusForCancel
    {
        public Guid Id { get; set; }
        public StatusSenha StatusSenha { get; set; } = StatusSenha.CANCELADA;
    }
}
