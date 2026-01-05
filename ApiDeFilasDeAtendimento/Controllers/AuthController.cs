using ApiDeFilasDeAtendimento.DTOs.Auth;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email!);
            if(user == null)
            {
                return Unauthorized("Credenciais invalidas!");

            }
            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password!, isPersistent: true, lockoutOnFailure: false);
            if(result.Succeeded)
            {
                return Ok(new
                {
                    Message = "Login realizado com sucesso!",
                    UserId = user.Id,
                    userName = user.UserName,
                    Email = user.Email,
                    LocalId = user.LocalId,
                });
            }
            if (result.IsLockedOut)
            {
                return BadRequest(new { Message = "Conta bloqueada temporariamente" });
            }

            return Unauthorized(new { Message = "Email ou senha inválidos" });
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email!);
            if (userExist is not null)
            {
                return BadRequest("Usuário existe!");
            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                LocalId = model.LocalId
            };
            var result = await _userManager.CreateAsync(user, model.Password!);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest("Ocorreu um erro ao criar o usuário!");
            }
            return Ok();
        }
    }
}
