using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.DTOs.Guiches
{
    public class GuicheCreateDto
    {
        public required string Nome { get; set; }
        public string FuncionarioId { get; set; }
        public Guid UnidadeId { get; set; }
    }
}
