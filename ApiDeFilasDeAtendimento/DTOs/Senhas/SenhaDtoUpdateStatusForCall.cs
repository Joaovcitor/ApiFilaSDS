using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.Senhas
{
    public class SenhaDtoUpdateStatusForCall
    {
        public Guid Id { get; set; }
        public StatusSenha StatusSenha { get; set; } = StatusSenha.CHAMADA;
        public int? GuicheId { get; set; }
        public string FuncionarioNome { get; set; } = string.Empty;
    }
}
