using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Guiches;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuicheController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GuicheController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GuicheCreateDto dados)
        {
            var guiche = _mapper.Map<Guiche>(dados);
           _context.Set<Guiche>().Add(guiche);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
