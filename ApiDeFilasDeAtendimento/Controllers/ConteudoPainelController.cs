using ApiDeFilasDeAtendimento.DTOs.ConteudosPainel;
using ApiDeFilasDeAtendimento.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers;

[ApiController]
[Route("api/conteudos-painel")]
[Authorize]
public class ConteudoPainelController : ControllerBase
{
    private readonly IConteudoPainelService _service;

    public ConteudoPainelController(
        IConteudoPainelService service)
    {
        _service = service;
    }

    /// <summary>
    /// Cadastra uma imagem ou vídeo para exibição no painel.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AcessoAdmin")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    [ProducesResponseType(
        typeof(ConteudoPainelResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConteudoPainelResponseDto>> Create(
        [FromForm] ConteudoPainelCreateDto dados)
    {
        var conteudo = await _service.CreateAsync(dados);

        return CreatedAtAction(
            nameof(GetById),
            new { id = conteudo.Id },
            conteudo);
    }

    /// <summary>
    /// Retorna todos os conteúdos das unidades do usuário logado.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "AcessoAdmin")]
    [ProducesResponseType(
        typeof(List<ConteudoPainelResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ConteudoPainelResponseDto>>>
        GetAll()
    {
        var conteudos = await _service.GetAllAsync();

        return Ok(conteudos);
    }

    /// <summary>
    /// Retorna um conteúdo pelo ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,SuperAdmin,Tv")]
    [ProducesResponseType(
        typeof(ConteudoPainelResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConteudoPainelResponseDto>>
        GetById(Guid id)
    {
        var conteudo = await _service.GetByIdAsync(id);

        return Ok(conteudo);
    }

    /// <summary>
    /// Retorna todos os conteúdos de uma unidade.
    /// </summary>
    [HttpGet("unidade/{unidadeId:guid}")]
    [Authorize(Roles = "Admin,SuperAdmin,Tv")]
    [ProducesResponseType(
        typeof(List<ConteudoPainelResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ConteudoPainelResponseDto>>>
        GetByUnidade(Guid unidadeId)
    {
        var conteudos = await _service.GetByUnidadeAsync(
            unidadeId);

        return Ok(conteudos);
    }

    /// <summary>
    /// Atualiza os dados ou substitui o arquivo do conteúdo.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AcessoAdmin")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    [ProducesResponseType(
        typeof(ConteudoPainelResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConteudoPainelResponseDto>>
        Update(
            Guid id,
            [FromForm] ConteudoPainelUpdateDto dados)
    {
        var conteudo = await _service.UpdateAsync(id, dados);

        return Ok(conteudo);
    }

    /// <summary>
    /// Exclui o conteúdo e o respectivo arquivo físico.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AcessoAdmin")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
    
    [HttpGet("tv")]
    [Authorize(Policy = "AcessoTv")]
    [ProducesResponseType(
        typeof(List<ConteudoPainelResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ConteudoPainelResponseDto>>>
        GetConteudosDaTv()
    {
        var conteudos = await _service.GetConteudosDaTvAsync();

        return Ok(conteudos);
    }
}