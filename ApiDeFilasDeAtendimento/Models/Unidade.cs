using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDeFilasDeAtendimento.Models
{
    [Table("Unidade")]
    public class Unidade
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public string Local { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];
        public ICollection<Guiche> Guiches { get; set; } = [];
        public ICollection<FilaSenha> FilasSenhas { get; set; } = [];

    }
}
