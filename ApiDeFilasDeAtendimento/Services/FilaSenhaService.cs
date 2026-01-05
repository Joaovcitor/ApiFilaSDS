using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Senhas;
using ApiDeFilasDeAtendimento.Enums;
using ApiDeFilasDeAtendimento.Hubs;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Services
{
    public class FilaSenhaService : IFilaSenhaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<QueueHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FilaSenhaService(AppDbContext context, IHubContext<QueueHub> hubContext, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<FilaSenha> CreateSenha(SenhaDtoCreate dados)
        {
            var userLogado = await _userManager.GetUserAsync(
            _httpContextAccessor.HttpContext!.User);
            var novaSenha = _mapper.Map<FilaSenha>(dados);

            var dataHoje = DateTime.UtcNow.Date;
            var ultimoNumero = await _context.Set<FilaSenha>()
                .Where(s => s.UnidadeId == dados.UnidadeId && s.DataCriacao.Date == dataHoje)
                .OrderByDescending(s => s.Numero)
                .Select(s => s.Numero)
                .FirstOrDefaultAsync();

            novaSenha.Numero = ultimoNumero + 1;
            novaSenha.DataCriacao = DateTime.UtcNow;
            novaSenha.UnidadeId = userLogado.LocalId;

            _context.Set<FilaSenha>().Add(novaSenha);
            await _context.SaveChangesAsync();

            await NotificarAtualizacaoFila(novaSenha.UnidadeId);

            return novaSenha;
        }
        public async Task<List<FilaSenha>> GetAguardando()
        {
            return await _context.Set<FilaSenha>().Where(s => s.StatusSenha == StatusSenha.AGUARDANDO).OrderBy(s => s.Prioritario ? 0 : 1).ThenBy(s => s.DataCriacao).ToListAsync();
        }
        public async Task<FilaSenha> UpdateStatusForCall(SenhaDtoUpdateStatusForCall dados)
        {
            var userLogado = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            var guiche = await _context.Guiche
            .Where(g => g.FuncionarioId == userLogado.Id)
            .Select(g => new { g.Id }) // só o que interessa
            .FirstOrDefaultAsync()
            ?? throw new Exception("Guichê não encontrado para este usuário");
            // Assumindo que o Guid Id vem dentro do DTO
            var senha = await _context.Set<FilaSenha>().FindAsync(dados.Id)
                        ?? throw new Exception("Senha não encontrada.");

            _mapper.Map(dados, senha);

            senha.StatusSenha = StatusSenha.CHAMADA;
            senha.DataChamada = DateTime.UtcNow;
            senha.QuantidadeDeChamadas++;
            senha.GuicheId = guiche.Id;

            await _context.SaveChangesAsync();

            // Preparar dados para o seu Hub (TicketCalled)
            var ultimasChamadas = await GetUltimasChamadas(senha.UnidadeId);

            // Disparar o método que você criou no QueueHub
            await _hubContext.Clients.All.SendAsync("TicketCalled", senha, ultimasChamadas);

            return senha;
        }

        public async Task<FilaSenha> UpdateStatusForAtendimento(SenhaDtoUpdateStatusForAtendimento dados)
        {
            var senha = await _context.Set<FilaSenha>().FindAsync(dados.Id)
                        ?? throw new Exception("Senha não encontrada.");

            _mapper.Map(dados, senha);
            senha.StatusSenha = StatusSenha.EM_ATENDIMENTO;
            // Aqui você pode registrar o início real do atendimento se tiver o campo

            await _context.SaveChangesAsync();
            return senha;
        }

        public async Task<FilaSenha> UpdateMotivoAtendimento(SenhaDtoUpdateMotivoAtendimento dados)
        {
            var senha = await _context.Set<FilaSenha>().FindAsync(dados.Id)
                        ?? throw new Exception("Senha não encontrada.");

            _mapper.Map(dados, senha);
            senha.StatusSenha = StatusSenha.FINALIZADA;
            senha.DataFinalizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return senha;
        }

        public async Task<FilaSenha> UpdateStatusForCancel(SenhaDtoUpdateStatusForCancel dados)
        {
            var senha = await _context.Set<FilaSenha>().FindAsync(dados.Id)
                        ?? throw new Exception("Senha não encontrada.");

            senha.StatusSenha = StatusSenha.CANCELADA;
            await _context.SaveChangesAsync();
            return senha;
        }

        public async Task<FilaSenha> UpdateNameEmployee(SenhaDtoUpdateNameEmployee dados)
        {
            var senha = await _context.Set<FilaSenha>().FindAsync(dados.Id)
                        ?? throw new Exception("Senha não encontrada.");

            _mapper.Map(dados, senha);
            await _context.SaveChangesAsync();
            return senha;
        }

        // Métodos auxiliares para manter o código limpo
        private async Task<List<FilaSenha>> GetUltimasChamadas(Guid unidadeId)
        {
            return await _context.Set<FilaSenha>()
                .Where(s => s.UnidadeId == unidadeId && s.StatusSenha == StatusSenha.CHAMADA)
                .OrderByDescending(s => s.DataChamada)
                .Take(5)
                .ToListAsync();
        }

        private async Task NotificarAtualizacaoFila(Guid unidadeId)
        {
            var waitingNormal = await _context.Set<FilaSenha>()
                .CountAsync(s => s.UnidadeId == unidadeId && s.StatusSenha == StatusSenha.AGUARDANDO && !s.Prioritario);

            var waitingPriority = await _context.Set<FilaSenha>()
                .CountAsync(s => s.UnidadeId == unidadeId && s.StatusSenha == StatusSenha.AGUARDANDO && s.Prioritario);

            await _hubContext.Clients.All.SendAsync("QueueUpdated", waitingNormal, waitingPriority);
        }
    }
}