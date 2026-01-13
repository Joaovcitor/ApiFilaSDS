using ApiDeFilasDeAtendimento.DTOs.Filters;
using ApiDeFilasDeAtendimento.DTOs.Pagination;
using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IReportService
    {
        Task<PagedResult<FilaSenha>> TodasAsSenhas(ReportFilter filtros);
        Task<PagedResult<FilaSenha>> SenhasDoUsuario(ReportFilter filtros);
    }
}
