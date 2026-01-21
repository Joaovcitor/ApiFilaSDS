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
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok(new { Message = "Você encerrou sua sessão com sucesso!" });
        }
        [HttpPost("pedir-nova-senha")]
        public async Task<IActionResult> SendResetPasswordAsync([FromBody] string email)
        {
            await _authService.RequestPasswordReset(email);
            return Ok(new { Message = "Pedido feito com sucesso" });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.ResetPassword(dto);

            return Ok(new { Message = "Senha redefinida com sucesso!" });
        }
    }
}
