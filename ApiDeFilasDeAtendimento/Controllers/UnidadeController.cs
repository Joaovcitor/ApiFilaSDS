using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.DTOs.Unidades;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadeController : ControllerBase
    {
        private readonly IUnidadeService _unidadeService;

        public UnidadeController(IUnidadeService unidadeService)
        {
            _unidadeService = unidadeService;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UnidadeDtoCreate dados)
        {
            var unidade = await _unidadeService.Create(dados);
            return Ok(dados);
        }
        [HttpGet]
        public async Task<IActionResult> GetUnidades()
        {
            var unidades = await _unidadeService.GetUnidadesDoDonoLogado();
            return Ok(unidades);
        }
        [HttpGet("buscar-unidade/{Id}")]
        public async Task<IActionResult> GetUnidade(Guid Id)
        {
            var unidade = await _unidadeService.GetById(Id);
            return Ok(unidade);
        }
    }
}
