using ApiDeFilasDeAtendimento.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiDeFilasDeAtendimento.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Unidade Local {  get; set; }
        public Guid LocalId { get; set; }
        public TipoAtendimento Atendimento { get; set; }
        public ApplicationUser? Dono { get; set; }
        public string? DonoId { get; set; }
        [StringLength(255)]
        public string NomeCompleto { get; set; } = null!;
    }
}
