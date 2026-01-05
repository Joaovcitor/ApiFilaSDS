using System.ComponentModel.DataAnnotations;

namespace ApiDeFilasDeAtendimento.DTOs.Auth
{
    public class RegisterModelDto
    {
        [Required(ErrorMessage = "Nome do usuário é obrigatório")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email é obrigatório")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatória")]
        public string? Password { get; set; }

        public Guid LocalId { get; set; }
    }
}
