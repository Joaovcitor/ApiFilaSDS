using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnidadeController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string local)
        {
            var unidade = new Unidade
            {
                Local = local
            };

            _context.Add(unidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), unidade);
        }
        [HttpGet]
        public async Task<IActionResult> GetUnidades()
        {
            var unidades = await _context.Unidade
                .AsNoTracking()
                .Include(u => u.FilasSenhas)
                .ToListAsync();

            return Ok(unidades);
        }
        [HttpGet("meu-guiche")]
        public async Task<IActionResult> GetGuicheUsuarioLogado()
        {
            var userLogado = await _userManager.GetUserAsync(User);

            if (userLogado == null) return Unauthorized();
            var guiche = await _context.Guiche
                .FirstOrDefaultAsync(g => g.FuncionarioId == userLogado.Id);

            if (guiche == null) return NotFound("Nenhum guichê vinculado a este funcionário.");

            return Ok(guiche);
        }
        [HttpGet("buscar-unidade/{Id}")]
        public async Task<IActionResult> GetUnidade(Guid Id)
        {
            var unidade = await _context.Unidade
            .Include(u => u.Guiches)
            .Include(u => u.FilasSenhas)
            .FirstOrDefaultAsync(u => u.Id == Id);
            if (unidade == null) return NotFound();

            return Ok(unidade);

        }
    }
}
