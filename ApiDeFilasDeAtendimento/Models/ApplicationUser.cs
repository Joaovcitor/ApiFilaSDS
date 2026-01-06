using ApiDeFilasDeAtendimento.Enums;
using Microsoft.AspNetCore.Identity;

namespace ApiDeFilasDeAtendimento.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid LocalId { get; set; }
        public TipoAtendimento Atendimento { get; set; }
    }
}
