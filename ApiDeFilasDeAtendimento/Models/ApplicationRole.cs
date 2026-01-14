using Microsoft.AspNetCore.Identity;

namespace ApiDeFilasDeAtendimento.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}
