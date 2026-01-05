using System.ComponentModel.DataAnnotations;

namespace ApiDeFilasDeAtendimento.DTOs.Auth
{
    public class LoginModelDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatória")]
        public string? Password { get; set; }
    }
}
