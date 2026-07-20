using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.Models;

[Table("ConteudoPainel")]
public class ConteudoPainel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public TipoConteudoPainel TipoConteudo { get; set; }

    [Required]
    [StringLength(500)]
    public string CaminhoArquivo { get; set; } = string.Empty;

    [StringLength(255)]
    public string? NomeArquivoOriginal { get; set; }

    [StringLength(255)]
    public string? NomeArquivoArmazenado { get; set; }

    [StringLength(100)]
    public string? ContentType { get; set; }

    public long? TamanhoBytes { get; set; }

    public int OrdemExibicao { get; set; }

    public int DuracaoExibicaoSegundos { get; set; } = 10;

    public bool Ativo { get; set; } = true;

    public DateTime? InicioExibicao { get; set; }

    public DateTime? FimExibicao { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public DateTime? AtualizadoEm { get; set; }

    [Required]
    public Guid UnidadeId { get; set; }

    public Unidade Unidade { get; set; } = null!;
}