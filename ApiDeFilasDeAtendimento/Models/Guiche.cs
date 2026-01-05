using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDeFilasDeAtendimento.Models
{
    [Table("Guiche")]
    public class Guiche
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Nome { get; set; }
        public ApplicationUser Funcionario { get; set; }
        public string FuncionarioId { get; set; }
        public Unidade Unidade { get; set; }
        public Guid UnidadeId {  get; set; }
    }
}
