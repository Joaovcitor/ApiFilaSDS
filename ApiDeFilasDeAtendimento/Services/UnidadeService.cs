using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Unidades;
using ApiDeFilasDeAtendimento.Exceptions;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Services
{
    public class UnidadeService : IUnidadeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnidadeService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IMapper mapper, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unidade> Create(UnidadeDtoCreate dados)
        {
            var unidade = _mapper.Map<Unidade>(dados);
            var usuarioLogado = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            unidade.DonoId = usuarioLogado!.Id;
            _context.Add(unidade);
            await _context.SaveChangesAsync();
            return unidade;
        }

        public async Task<Unidade> GetById(Guid id)
        {
            var unidade = await _context.Unidade.Include(u => u.ApplicationUsers).FirstOrDefaultAsync(u => u.Id == id);
            return unidade ?? throw new NotFoundException("Unidade não encontrada");
        }

        public async Task<List<Unidade>> GetUnidadesDoDonoLogado()
        {
            var usuarioLogado = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            var unidades = await _context.Unidade.AsNoTracking().Where(s => s.DonoId == usuarioLogado.Id).ToListAsync();
            return unidades;
        }
    }
}
