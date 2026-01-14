using ApiDeFilasDeAtendimento.DTOs.Auth;
using ApiDeFilasDeAtendimento.DTOs.Managements;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Garante que apenas usuários autenticados acessem
public class ManagementController : ControllerBase
{
    private readonly IManagementService _managementService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ManagementController(IManagementService managementService, UserManager<ApplicationUser> userManager)
    {
        _managementService = managementService;
        _userManager = userManager;
    }

    [HttpGet("meus-usuarios")]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var donoId = _userManager.GetUserId(User) ?? throw new UnauthorizedAccessException();

        var users = await _managementService.ListarMeusUsuariosAsync(donoId, page, pageSize);
        return Ok(users);
    }

    [HttpGet("usuario/{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var donoId = _userManager.GetUserId(User) ?? throw new UnauthorizedAccessException();

        var userById = await _managementService.ObterPorIdAsync(id, donoId);
        return Ok(userById);
    }

    [HttpPost("criar-usuario")]
    public async Task<IActionResult> Post([FromBody] RegisterModelDto dto, [FromQuery] string role)
    {
        var donoId = _userManager.GetUserId(User) ?? throw new UnauthorizedAccessException();

        await _managementService.CriarUsuarioAsync(dto, donoId, role);

        return StatusCode(201, new { Message = "Usuário criado com sucesso" });
    }

    [HttpPut("atualizar-usuario/{id}")]
    public async Task<IActionResult> Atualizar([FromRoute] string id, [FromBody] UserDtoUpdate dados)
    {
        var donoId = _userManager.GetUserId(User) ?? throw new UnauthorizedAccessException();

        await _managementService.AtualizarAsync(id, dados, donoId);
        return Ok(new { Message = "Usuário atualizado com sucesso" });
    }
}