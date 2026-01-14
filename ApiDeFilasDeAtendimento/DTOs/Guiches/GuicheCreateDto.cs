using ApiDeFilasDeAtendimento.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiDeFilasDeAtendimento.DTOs.Guiches
{
    public class GuicheCreateDto
    {
        [StringLength(255)]
        public required string Nome { get; set; }
        [Required(ErrorMessage = "Funcionário é obrigatório")]
        public required string FuncionarioId { get; set; }
        [Required(ErrorMessage = "A unidade é obrigatória")]
        public Guid UnidadeId { get; set; }
        [Required(ErrorMessage = "O Dono do guichê é obrigatório")]
        public required string DonoId { get; set; }

    }
}
