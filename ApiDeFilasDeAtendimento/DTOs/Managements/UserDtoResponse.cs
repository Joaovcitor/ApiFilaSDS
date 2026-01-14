using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.Managements
{
    public class UserDtoResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? NomeCompleto { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public TipoAtendimento Atendimento {  get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
