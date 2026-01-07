using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Guiches;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Services
{
    public class GuicheService : IGuicheService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;


        public GuicheService(IHttpContextAccessor httpContextAccessor, IMapper mapper, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Guiche> CreateGuiche(GuicheCreateDto dados)
        {
            var guiche = _mapper.Map<Guiche>(dados);
            _context.Set<Guiche>().Add(guiche);
            await _context.SaveChangesAsync();
            return guiche;
        }

        public async Task<Guiche> GuicheDoUsuario()
        {
            var userLogado = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            var guiche = await _context.Guiche.FirstOrDefaultAsync(g => g.FuncionarioId == userLogado.Id);
            
            return guiche;
        }
    }
}
