using ApiDeFilasDeAtendimento.DTOs.Senhas;
using ApiDeFilasDeAtendimento.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilaSenhaController : ControllerBase
    {
        private readonly IFilaSenhaService _filaService;

        public FilaSenhaController(IFilaSenhaService filaService)
        {
            _filaService = filaService;
        }

        [HttpPost("gerar-senha")]
        public async Task<IActionResult> Criar([FromBody] SenhaDtoCreate dados)
        {
            var senha = await _filaService.CreateSenha(dados);
            return CreatedAtAction(nameof(Criar), new { id = senha.Id }, senha);
        }

        [HttpGet("aguardando")]
        public async Task<IActionResult> GetAguardando()
        {
            var senhas = await _filaService.GetAguardando();
            return Ok(senhas);
        }

        [HttpPut("chamar-proximo")]
        public async Task<IActionResult> Chamar([FromBody] SenhaDtoUpdateStatusForCall dados)
        {
            try
            {
                var senha = await _filaService.UpdateStatusForCall(dados);
                return Ok(senha);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("iniciar-atendimento")]
        public async Task<IActionResult> Atender([FromBody] SenhaDtoUpdateStatusForAtendimento dados)
        {
            try
            {
                var senha = await _filaService.UpdateStatusForAtendimento(dados);
                return Ok(senha);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("finalizar-atendimento")]
        public async Task<IActionResult> Finalizar([FromBody] SenhaDtoUpdateMotivoAtendimento dados)
        {
            var senha = await _filaService.UpdateMotivoAtendimento(dados);
            return Ok(senha);
        }

        [HttpPut("cancelar-senha")]
        public async Task<IActionResult> Cancelar([FromBody] SenhaDtoUpdateStatusForCancel dados)
        {
            var senha = await _filaService.UpdateStatusForCancel(dados);
            return Ok(senha);
        }

        [HttpPut("atualizar-funcionario")]
        public async Task<IActionResult> AtualizarFuncionario([FromBody] SenhaDtoUpdateNameEmployee dados)
        {
            var senha = await _filaService.UpdateNameEmployee(dados);
            return Ok(senha);
        }
        [HttpGet("senhas-do-funcionario")]
        public async Task<IActionResult> SenhasDoFuncionario()
        {
            var quantidadeDeSenhas = await _filaService.GetSenhasAtendidasPeloUsuario();
            return Ok(quantidadeDeSenhas);
        }
    }
}