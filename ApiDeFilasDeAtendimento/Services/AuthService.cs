using ApiDeFilasDeAtendimento.DTOs.Auth;
using ApiDeFilasDeAtendimento.Exceptions;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiDeFilasDeAtendimento.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<object> Login(LoginModelDto dados)
        {
            var user = await _userManager.FindByEmailAsync(dados.Email!) ?? throw new BadRequestException("Credênciais Invalidas.");
            var result = await _signInManager.PasswordSignInAsync(user, dados.Password!, isPersistent: true, lockoutOnFailure: true);

            if (result.IsLockedOut)
                throw new UnauthorizedAccessException("Conta bloqueada temporariamente por exesso de tentativas!");
            if (!result.Succeeded)
                throw new BadRequestException("Credenciais Invalidas");
            var roles = await _userManager.GetRolesAsync(user);

            return new
            {
                Message = "Login realizado com sucesso",
                user.Id,
                user.UserName,
                user.NomeCompleto,
                user.LocalId,
                Roles = roles,
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
