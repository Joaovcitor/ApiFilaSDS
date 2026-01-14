using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Filters;
using ApiDeFilasDeAtendimento.DTOs.Pagination;
using ApiDeFilasDeAtendimento.Exceptions;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiDeFilasDeAtendimento.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportService(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<PagedResult<FilaSenha>> SenhasDoUsuario(ReportFilter filtro)
        {
            var userLogado = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User)
                ?? throw new UnauthorizedAccessException("Você deve fazer login");
            var guiche = await _context.Guiche.FirstOrDefaultAsync(g => g.FuncionarioId == userLogado!.Id)
                ?? throw new NotFoundException("GUicê não encontrado!");
            var query = _context.FilaSenha.Where(s => s.GuicheId == guiche.Id).AsQueryable();
            if (filtro.DataInicio.HasValue)
            {
                query = query.Where(x => x.DataCriacao >= filtro.DataInicio.Value);
            }
            if (filtro.DataFim.HasValue)
            {
                query = query.Where(x => x.DataCriacao <= filtro.DataFim.Value);
            }
            var totalRegistros = await query.CountAsync();
            var itens = await query.Where(x => x.GuicheId == guiche.Id)
                .OrderByDescending(x => x.DataCriacao)
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToListAsync();

            return new PagedResult<FilaSenha>
            {
                Items = itens,
                Page = filtro.Page,
                PageSize = filtro.PageSize,
                TotalItems = totalRegistros,
            };
        }

        public async Task<PagedResult<FilaSenha>> TodasAsSenhas(ReportFilter filtro)
        {
            var senhas = _context.FilaSenha.AsQueryable();
            if (filtro.DataInicio.HasValue)
            {
                senhas = senhas.Where(x => x.DataCriacao >= filtro.DataInicio.Value);
            }
            if (filtro.DataFim.HasValue)
            {
                senhas = senhas.Where(x => x.DataCriacao <= filtro.DataFim.Value);
            }
            if (filtro.UnidadeId.HasValue)
            {
                senhas = senhas.Where(x => x.UnidadeId == filtro.UnidadeId.Value);
            }
            if (!string.IsNullOrWhiteSpace(filtro.UsuarioId))
            {
                senhas = senhas.Where(x => x.FuncionarioId == filtro.UsuarioId);
            }
            var totalRegistros = senhas.Count();

            var items = await senhas
            .OrderByDescending(x => x.DataCriacao)
            .Skip((filtro.Page - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .ToListAsync();

            return new PagedResult<FilaSenha>
            {
                Items = items,
                TotalItems = totalRegistros,
                Page = filtro.Page,
                PageSize = filtro.PageSize
            };
        }
    }
}
