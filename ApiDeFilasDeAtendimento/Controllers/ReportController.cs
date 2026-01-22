using ApiDeFilasDeAtendimento.DTOs.Filters;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(IReportService reportService, UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _userManager = userManager;
        }
        [HttpPost("todas-as-senhas")]
        [Authorize(Policy = "AcessoAdmin")]
        public async Task<IActionResult> GetSenhas([FromBody] ReportFilter filtros)
        {
            var senhas = await _reportService.TodasAsSenhas(filtros);
            return Ok(senhas);
        }
        [HttpPost("relatorios-senhas-usuario-logado")]
        [Authorize(Policy = "AcessoOperacional")]
        public async Task<IActionResult> RelatorioUsuarioLogado([FromBody] ReportFilter filtros)
        {
            var senhas = await _reportService.SenhasDoUsuario(filtros);
            return Ok(senhas);
        }
        [HttpGet("tempo-medio-de-atendimento")]
        [Authorize(Policy = "AcessoAdmin")]
        public async Task<IActionResult> GetTempoMedioDeAtendimento()
        {
            var donoId = await _userManager.GetUserAsync(User) 
                ?? throw new UnauthorizedAccessException("Você deve fazer login.");
            var tempo = await _reportService.TempoDeAtendimento(donoId.Id);
            return Ok(tempo);
        }

        [HttpGet("tempo-medio-de-espera")]
        public async Task<IActionResult> GetTempoMedioDeEsperaParaAtendimento()
        {
            var donoId = await _userManager.GetUserAsync(User)
                ?? throw new UnauthorizedAccessException("Você deve fazer login.");
            var tempo = await _reportService.TempoMedioDeEspera(donoId.DonoId!);
            return Ok(tempo);
        }
    }
}
