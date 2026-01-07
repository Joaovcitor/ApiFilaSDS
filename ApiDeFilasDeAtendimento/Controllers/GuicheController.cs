using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.Guiches;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using ApiDeFilasDeAtendimento.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GuicheController : ControllerBase
    {
        private readonly IGuicheService _guicheService;

        public GuicheController(IGuicheService guicheService)
        {
            _guicheService = guicheService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GuicheCreateDto dados)
        {
            var guiche = await _guicheService.CreateGuiche(dados);
            return Ok();
        }
        [HttpGet("guiche-usuario")]
        public async Task<IActionResult> GetGuicheUsuario()
        {
            var guiche = await _guicheService.GuicheDoUsuario();
            return Ok(guiche);
        }
    }
}
