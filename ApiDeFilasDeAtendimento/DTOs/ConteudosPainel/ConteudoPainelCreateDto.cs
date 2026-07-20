using System.ComponentModel.DataAnnotations;
using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.ConteudosPainel;

public class ConteudoPainelCreateDto
{
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo do conteúdo é obrigatório")]
    public TipoConteudoPainel TipoConteudo { get; set; }

    [Required(ErrorMessage = "A unidade é obrigatória")]
    public Guid UnidadeId { get; set; }

    [Required(ErrorMessage = "O arquivo é obrigatório")]
    public IFormFile Arquivo { get; set; } = null!;

    [Range(1, 3600)]
    public int DuracaoExibicaoSegundos { get; set; } = 10;

    public int OrdemExibicao { get; set; }

    public DateTime? InicioExibicao { get; set; }

    public DateTime? FimExibicao { get; set; }
}