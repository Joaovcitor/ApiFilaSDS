using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDeFilasDeAtendimento.Models
{
    [Table("Unidade")]
    public class Unidade
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        [Required(ErrorMessage = "O Local da unidade é obrigatório")]
        [StringLength(255)]
        public string Local { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];
        public ICollection<Guiche> Guiches { get; set; } = [];
        public ICollection<FilaSenha> FilasSenhas { get; set; } = [];
        public ApplicationUser? Dono { get; set; }
        [Required(ErrorMessage = "O ID do Dono é obrigatório")]
        public string? DonoId { get; set; }

    }
}
