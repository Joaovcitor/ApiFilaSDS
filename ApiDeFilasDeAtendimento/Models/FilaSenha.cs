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

        public int? GuicheId { get; set; }
        public Guid UnidadeId {  get; set; }

        public bool Prioritario { get; set; } = false;

        // NOVO: Tipo de atendimento
        public TipoAtendimento TipoAtendimento { get; set; } = TipoAtendimento.CADASTRO_UNICO;

        [StringLength(255)]
        public string? FuncionarioNome { get; set; }

        [StringLength(500)]
        public string? MotivoAtendimento { get; set; }

        public StatusSenha StatusSenha { get; set; } = StatusSenha.AGUARDANDO;

        public int QuantidadeDeChamadas { get; set; } = 0;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataChamada { get; set; }
        public DateTime? DataFinalizacao { get; set; }

        [NotMapped]
        public TimeSpan? TempoAtendimento =>
            DataFinalizacao.HasValue && DataChamada.HasValue
                ? DataFinalizacao - DataChamada
                : null;

        // NOVO: Helper para label do local (Sala vs Guichê)
        [NotMapped]
        public string LocalLabel => TipoAtendimento == TipoAtendimento.CADASTRO_UNICO ? "Guichê" : "Sala";
    }
}
