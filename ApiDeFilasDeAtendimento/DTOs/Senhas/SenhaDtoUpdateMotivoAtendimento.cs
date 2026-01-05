namespace ApiDeFilasDeAtendimento.DTOs.Senhas
{
    public class SenhaDtoUpdateMotivoAtendimento
    {
        public Guid Id { get; set; }
        public required string MotivoAtendimento { get; set; }
    }
}
