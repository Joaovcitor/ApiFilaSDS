using System.ComponentModel.DataAnnotations;

namespace ApiDeFilasDeAtendimento.DTOs.Guiches
{
    public class GuicheDtoUpdate
    {
        [StringLength(255)]
        public string? Nome { get; set; }
    }
}
