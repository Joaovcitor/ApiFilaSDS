using ApiDeFilasDeAtendimento.DTOs.Filters;
using ApiDeFilasDeAtendimento.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpPost("todas-as-senhas")]
        public async Task<IActionResult> GetSenhas([FromBody] ReportFilter filtros)
        {
            var senhas = await _reportService.TodasAsSenhas(filtros);
            return Ok(senhas);
        }
        [HttpPost("relatorios-senhas-usuario-logado")]
        public async Task<IActionResult> RelatorioUsuarioLogado([FromBody] ReportFilter filtros)
        {
            var senhas = await _reportService.SenhasDoUsuario(filtros);
            return Ok(senhas);
        }
    }
}
