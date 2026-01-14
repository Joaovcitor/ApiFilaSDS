using ApiDeFilasDeAtendimento.DTOs.Auth;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto loginModel)
        {
            var user = await _authService.Login(loginModel);
            return Ok(user);
        }
        //[HttpPost]
        //[Authorize]
        //[Route("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModelDto model, string roleSolicitada)
        //{
        //    var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("Você deve fazer login!");
        //    await _authService.RegistrarUsuario(model, roleSolicitada, adminId!);
        //    return Ok(new { Message = "Funcionário cadastrado com sucesso" });
        //}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok(new { Message = "Você encerrou sua sessão com sucesso!" });
        }
    }
}
