using ApiDeFilasDeAtendimento.DTOs.Auth;
using ApiDeFilasDeAtendimento.DTOs.Managements;
using ApiDeFilasDeAtendimento.DTOs.Pagination;
using ApiDeFilasDeAtendimento.Exceptions;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Services
{
    public class ManagementService : IManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagementService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AtualizarAsync(string id, UserDtoUpdate dados, string donoId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id && u.DonoId == donoId) 
                ?? throw new NotFoundException("Usuário não encontrado");
            user.NomeCompleto = dados.NomeCompleto;
            user.LocalId = dados.LocalId;
            user.Email = dados.Email;

            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                var erro = result.Errors.First().Description;
                throw new BusinessException($"Erro ao atualizar: {erro}");
            }
            return result;
        }

        public async Task<IdentityResult> CriarUsuarioAsync(RegisterModelDto dados, string donoId, string role)
        {
            var userExist = await _userManager.FindByEmailAsync(dados.Email!);
            if (userExist is not null)
                throw new ConflictException("Este e-mail já está cadastrado.");

            var user = new ApplicationUser
            {
                Email = dados.Email,
                UserName = dados.UserName,
                LocalId = dados.LocalId,
                DonoId = donoId,
                NomeCompleto = dados.NomeCompleto,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, dados.Password!);

            if (!result.Succeeded)
            {
                var erro = result.Errors.First().Description;
                throw new BusinessException($"Erro ao criar usuário: {erro}");
            }

            await _userManager.AddToRoleAsync(user, role);
            return result;
        }

        public async Task<PagedResult<UserDtoResponse>> ListarMeusUsuariosAsync(string donoId, int page = 1, int pageSize = 10)
        {
            var query = _userManager.Users
                .Where(u => u.DonoId == donoId)
                .AsNoTracking();

            var totalItems = await query.CountAsync();

            var usuariosBrutos = await query
                .OrderBy(u => u.NomeCompleto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var itens = new List<UserDtoResponse>();

            foreach (var u in usuariosBrutos)
            {
                var roles = await _userManager.GetRolesAsync(u); 
                itens.Add(new UserDtoResponse
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    NomeCompleto = u.NomeCompleto,
                    Roles = roles.ToList()
                });
            }

            return new PagedResult<UserDtoResponse>
            {
                Items = itens,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<UserDtoResponse> ObterPorIdAsync(string id, string donoId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id && u.DonoId == donoId) 
                ?? throw new NotFoundException("Usuário não encontrado");
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDtoResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                NomeCompleto = user.NomeCompleto,
                Roles = roles.ToList()
            };
        }
    }
}
