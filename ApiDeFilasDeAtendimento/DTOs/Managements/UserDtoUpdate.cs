using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.Managements
{
    public class UserDtoUpdate
    {
        public Guid LocalId {  get; set; }
        public string? UserName { get; set; }
        public string? NomeCompleto { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public TipoAtendimento? Atendimento { get; set; }
    }
}
