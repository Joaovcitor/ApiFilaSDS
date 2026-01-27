using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDeFilasDeAtendimento.Models
{
    [Table("TiposDeAtendimento")]
    public class TiposDeAtendimento
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Você deve colocar o nome do atendimento")]
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        [Required(ErrorMessage = "DonoID é obrigatório")]
        public string DonoId { get; set; }
        [ForeignKey("DonoId")]
        public virtual ApplicationUser Dono {  get; set; }
        public ICollection<FilaSenha> Senhas { get; set; } = [];
        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
