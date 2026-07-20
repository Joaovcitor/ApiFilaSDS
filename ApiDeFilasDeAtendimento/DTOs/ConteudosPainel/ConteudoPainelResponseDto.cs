using ApiDeFilasDeAtendimento.Enums;

namespace ApiDeFilasDeAtendimento.DTOs.ConteudosPainel;

public class ConteudoPainelResponseDto
{
    public Guid Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public TipoConteudoPainel TipoConteudo { get; set; }

    public string CaminhoArquivo { get; set; } = string.Empty;

    public string? NomeArquivoOriginal { get; set; }

    public string? ContentType { get; set; }

    public long? TamanhoBytes { get; set; }

    public int OrdemExibicao { get; set; }

    public int DuracaoExibicaoSegundos { get; set; }

    public bool Ativo { get; set; }

    public DateTime? InicioExibicao { get; set; }

    public DateTime? FimExibicao { get; set; }

    public DateTime CriadoEm { get; set; }

    public Guid UnidadeId { get; set; }

    public string Unidade { get; set; } = string.Empty;
}