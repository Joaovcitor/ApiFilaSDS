using System.ComponentModel.DataAnnotations;

namespace ApiDeFilasDeAtendimento.DTOs.Senhas
{
    public class SenhaDtoUpdateNameEmployee
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FuncionarioNome { get; set; } = string.Empty;
    }
}
