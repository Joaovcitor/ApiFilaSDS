using ApiDeFilasDeAtendimento.Enums;
using System.ComponentModel.DataAnnotations;

public class SenhaDtoCreate
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório")]
    [StringLength(255)]
    public string NomeUsuarioCompleto { get; set; } = string.Empty;

    public bool Prioritario { get; set; } = false;

    [Required]
    public TipoAtendimento TipoAtendimento { get; set; }

    [Required]
    public Guid UnidadeId { get; set; }
}
