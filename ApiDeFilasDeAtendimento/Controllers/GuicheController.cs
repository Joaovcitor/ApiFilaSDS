using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Guiches;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuicheController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GuicheController(AppDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GuicheCreateDto dados)
        {
            var guiche = _mapper.Map<Guiche>(dados);
           _context.Set<Guiche>().Add(guiche);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("guiche-usuario")]
        public async Task<IActionResult> GetGuicheUsuario()
        {
            var usuarioLogado = await _userManager.GetUserAsync(User);
            if (usuarioLogado == null) return Unauthorized();
            var guiche = _context.Guiche.Where(g => g.FuncionarioId == usuarioLogado.Id);
            return Ok(guiche);
        }
    }
}
