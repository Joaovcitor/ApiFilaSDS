using ApiDeFilasDeAtendimento.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDeFilasDeAtendimento.Models
{
    [Table("FilaSenha")]
    public class FilaSenha
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(255)]
        public required string NomeUsuarioCompleto { get; set; }

        public int Numero { get; set; }

        [StringLength(10)]
        public string SenhaFormatada => $"{(Prioritario ? "P" : "N")}{Numero:D3}";
        public Guiche Guiche { get; set; }
        public int? GuicheId { get; set; }
        public Guid UnidadeId {  get; set; }

        public bool Prioritario { get; set; } = false;

        [StringLength(255)]
        public string? FuncionarioNome { get; set; }
        public string? FuncionarioId {  get; set; }

        [StringLength(500)]
        public string? MotivoAtendimento { get; set; }

        public StatusSenha StatusSenha { get; set; } = StatusSenha.AGUARDANDO;

        public int QuantidadeDeChamadas { get; set; } = 0;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataChamada { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        [StringLength(11)]
        public string? Cpf {  get; set; }
        public virtual ApplicationUser Dono { get; set; }
        public string? DonoId { get; set; }
        public Guid? TipoAtendimentoId { get; set; }
        [ForeignKey("TipoAtendimentoId")]
        public virtual TiposDeAtendimento? TipoDeAtendimento { get; set; }

        [NotMapped]
        public TimeSpan? TempoAtendimento =>
            DataFinalizacao.HasValue && DataChamada.HasValue
                ? DataFinalizacao - DataChamada
                : null;

    }
}
