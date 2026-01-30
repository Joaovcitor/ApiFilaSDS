using ApiDeFilasDeAtendimento.DTOs.Auth;
using ApiDeFilasDeAtendimento.Exceptions;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace ApiDeFilasDeAtendimento.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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

        public async Task<object> Me()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User)
                ?? throw new UnauthorizedException("Você deve realizar login");
            var roles = await _userManager.GetRolesAsync(user);
            return new
            {
                user.Id,
                user.UserName,
                user.NomeCompleto,
                user.LocalId,
                Roles = roles,
            };
        }

        public async Task RequestPasswordReset(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendPasswordResetAsync(user.Email!, token);
        }

        public async Task ResetPassword(ResetPasswordDto dados)
        {
            var user = await _userManager.FindByEmailAsync(dados.Email) 
                ?? throw new BadRequestException("Solicitação inválida");
            var result = await _userManager.ResetPasswordAsync(user, dados.Token, dados.NewPassword);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Erro ao resetar senha.";
                throw new BadRequestException(error);
            }

        }
    }
}
