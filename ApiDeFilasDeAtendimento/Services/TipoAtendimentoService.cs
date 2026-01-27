using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Atendimentos;
using ApiDeFilasDeAtendimento.Enums;
using ApiDeFilasDeAtendimento.Exceptions;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Services
{
    public class TipoAtendimentoService : ITipoAtendimentosService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TipoAtendimentoService(IMapper mapper, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<TiposDeAtendimento> Create(TipoDeAtendimentoDtoCreate dados)
        {
            var userLogado = await GetUserLogged();
            var atendimento = _mapper.Map<TiposDeAtendimento>(dados);
            atendimento.DonoId = userLogado.Id;
            atendimento.NormalizedName = dados.Name.ToUpper();
            _appDbContext.Set<TiposDeAtendimento>().Add(atendimento);
            await _appDbContext.SaveChangesAsync();
            return atendimento;
        }

        public async Task<List<TipoAtendimentoDtoResponse>> GetAll()
        {
            var userLogado = await GetUserLogged();
            var tiposAtendimento = await _appDbContext.TiposAtendimento.AsNoTracking()
                .Where(s => s.DonoId == userLogado.DonoId || userLogado.Id == s.DonoId).ToListAsync() 
                ?? throw new NotFoundException("Não foram encontrados tipos de atendimento");
            var tiposAtendimentoDto = _mapper.Map<List<TipoAtendimentoDtoResponse>>(tiposAtendimento);
            return tiposAtendimentoDto;
        }

        public async Task<TiposDeAtendimento> GetById(Guid id)
        {
            var userLogado = await GetUserLogged();
            var tipoAtendimento = await _appDbContext.TiposAtendimento.AsNoTracking()
                .Where(s => s.DonoId == userLogado.Id && s.Id == id).FirstOrDefaultAsync();
            return tipoAtendimento ?? throw new NotFoundException($"Tipo de atendimento com id {id} não encontrado.");
        }

        public async Task<TipoAtendimentoDtoResponse> Update(Guid Id, TipoAtendimentoDtoUpdate dados)
        {
            var userLogado = await GetUserLogged();
            var atendimento = await _appDbContext.TiposAtendimento
                .FirstOrDefaultAsync(a => a.Id == Id && a.DonoId == userLogado.Id);
            if (atendimento == null)
            {
                throw new NotFoundException($"Tipo de atendimento com id {Id} não encontrado ou não pertence a você.");
            }
            var atendimentoDto = _mapper.Map(dados, atendimento);
            atendimento.NormalizedName = dados.Name.ToUpper();
            _appDbContext.Update(atendimento);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<TipoAtendimentoDtoResponse>(atendimento);
        }

        private async Task<ApplicationUser> GetUserLogged()
        {
            var userLogado = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User)
                ?? throw new UnauthorizedException("Você deve fazer login");
            return userLogado;
        }
    }
}
