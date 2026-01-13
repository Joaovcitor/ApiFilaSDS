using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.Filters
{
    public class ReportFilter
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public Guid? UnidadeId { get; set; }
        public string? UsuarioId { get; set; }
        public StatusSenha Status { get; set; }

        // Parâmetros de paginação inclusos
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
