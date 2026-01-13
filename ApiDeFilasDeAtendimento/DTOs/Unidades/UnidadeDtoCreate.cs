using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.DTOs.Unidades
{
    public class UnidadeDtoCreate
    {
        public string Local { get; set; }
        public ApplicationUser? Dono { get; set; }
        public string? DonoId { get; set; }
    }
}
