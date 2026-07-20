using System.ComponentModel.DataAnnotations;
using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.ConteudosPainel;

public class ConteudoPainelUpdateDto
{
    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public TipoConteudoPainel TipoConteudo { get; set; }

    public IFormFile? Arquivo { get; set; }

    [Range(1, 3600)]
    public int DuracaoExibicaoSegundos { get; set; } = 10;

    public int OrdemExibicao { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime? InicioExibicao { get; set; }

    public DateTime? FimExibicao { get; set; }
}