using ApiDeFilasDeAtendimento.DTOs.Atendimentos;
using ApiDeFilasDeAtendimento.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TipoAtendimentoController : ControllerBase
    {
        private readonly ITipoAtendimentosService _tipoAtendimentoService;

        public TipoAtendimentoController(ITipoAtendimentosService tipoAtendimentoService)
        {
            _tipoAtendimentoService = tipoAtendimentoService;
        }

        [HttpPost]
        [Authorize(Policy = "AcessoAdmin")]
        public async Task<IActionResult> Post([FromBody] TipoDeAtendimentoDtoCreate dados)
        {
            var tipoAtendimento = await _tipoAtendimentoService.Create(dados);
            return Ok(tipoAtendimento);
        }
        [HttpGet("meus-tipos-de-atendimento")]
        public async Task<IActionResult> Get()
        {
            var tipoAtendimento = await _tipoAtendimentoService.GetAll();
            return Ok(tipoAtendimento);
        }
        [HttpGet("tipo-atendimento/{Id}")]
        [Authorize(Policy = "AcessoAdmin")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var tipoAtendimento = await _tipoAtendimentoService.GetById(Id);
            return Ok(tipoAtendimento);
        }
        [HttpPut("tipo-atendiemento/atualizar/{Id}")]
        [Authorize(Policy = "AcessoAdmin")]
        public async Task<IActionResult> Update(Guid Id, TipoAtendimentoDtoUpdate dados)
        {
            var tipoAtendimento = await _tipoAtendimentoService.Update(Id, dados);
            return Ok(tipoAtendimento);
        }
    }
}
