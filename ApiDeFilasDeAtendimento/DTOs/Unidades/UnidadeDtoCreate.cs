using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.DTOs.Unidades
{
    public class UnidadeDtoCreate
    {
        public string Local { get; set; }
        public string Codigo { get; set; } = string.Empty;
    }
}
