using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDeFilasDeAtendimento.Models
{
    [Table("Guiche")]
    public class Guiche
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do guichê é obrigatório!")]
        [StringLength(255)]
        public required string Nome { get; set; }
        public ApplicationUser Funcionario { get; set; }
        [Required(ErrorMessage = "O ID do funcionário é obirgatório!")]
        public string FuncionarioId { get; set; }
        public Unidade Unidade { get; set; }
        [Required(ErrorMessage = "O ID da Unidade é obrigatório!")]
        public Guid UnidadeId {  get; set; }
        public ApplicationUser? Dono { get; set; }
        [Required(ErrorMessage = "O ID do dono do guichê é obrigatório!")]
        public string? DonoId { get; set; }
    }
}
