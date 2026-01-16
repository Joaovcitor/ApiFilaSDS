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
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ManagementService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AdicionarNovasRoles(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return IdentityResult.Failed(new IdentityError { Description = "O nome da Permissão não pode ser vázio." });
            }
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
                return await _roleManager.CreateAsync(new ApplicationRole(name));
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> AdicionarRoleAoUsuario(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id)
                ?? throw new NotFoundException("Usuário não encontrado");
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return IdentityResult
                    .Failed(new IdentityError { Description = $"A Permissão {role} não existe no sistema, entre em contato com o administrador" });
            }
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> AtualizarAsync(string id, UserDtoUpdate dados, string donoId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id && u.DonoId == donoId)
                ?? throw new NotFoundException("Usuário não encontrado");
            if (string.IsNullOrWhiteSpace(dados.NomeCompleto))
            {
                throw new BadRequestException("Nome Completo não pode ser vázio");
            }
            user.NomeCompleto = dados.NomeCompleto;
            user.LocalId = dados.LocalId;
            user.Email = dados.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var erro = result.Errors.First().Description;
                throw new BusinessException($"Erro ao atualizar: {erro}");
            }
            return result;
        }

        public async Task<IdentityResult> CriarUsuarioAsync(RegisterModelDto dados, string donoId, string role)
        {
            if (string.IsNullOrWhiteSpace(role)) throw new BadRequestException("Permissão não pode ficar vázio");
            var userExist = await _userManager.FindByEmailAsync(dados.Email!);
            if (userExist is not null)
                throw new ConflictException("Este e-mail já está cadastrado.");
            var dono = await _userManager.FindByIdAsync(donoId)
                ?? throw new NotFoundException("Usuário não localizado");
            var roleUser = await _userManager.GetRolesAsync(dono)
                ?? throw new NotFoundException("Permissão não localizada");
            if (role == "Admin" && !roleUser.Contains("SuperAdmin"))
            {
                throw new ForbiddenException("Você não tem permissão para criar um usuário Admin");
            }
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

        public async Task<PagedResult<UserDtoResponse>> ListarUsuariosParaSuperAdmin(string superAdminId, int page, int pageSize)
        {
            var role = await _roleManager.FindByIdAsync(superAdminId);
            if (role != null && role.Name != "SuperAdmin")
            {
                throw new UnauthorizedAccessException("Você não tem autorização para usar esse recurso");
            }
            var query = _userManager.Users.AsNoTracking();
            var totalItems = await query.CountAsync();
            var dadosBrutos = await query.OrderBy(u => u.NomeCompleto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var itens = new List<UserDtoResponse>();
            foreach (var u in dadosBrutos)
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

        public Task<IdentityResult> RevogarAcesso(string id)
        {
            // ainda vou ver como vai funcionar esse metodo, que será usado exclusivamente pelo SuperAdmin
            throw new NotImplementedException();
        }
    }
}
