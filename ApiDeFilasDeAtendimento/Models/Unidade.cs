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
        ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];
        ICollection<Guiche> Guiches { get; set; } = [];
        ICollection<FilaSenha> FilasSenhas { get; set; } = [];

    }
}
