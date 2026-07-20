using ApiDeFilasDeAtendimento.DTOs.ConteudosPainel;

namespace ApiDeFilasDeAtendimento.Interfaces;

public interface IConteudoPainelService
{
    Task<ConteudoPainelResponseDto> CreateAsync(
        ConteudoPainelCreateDto dados);

    Task<List<ConteudoPainelResponseDto>> GetAllAsync();

    Task<List<ConteudoPainelResponseDto>> GetByUnidadeAsync(
        Guid unidadeId);

    Task<ConteudoPainelResponseDto> GetByIdAsync(
        Guid id);

    Task<ConteudoPainelResponseDto> UpdateAsync(
        Guid id,
        ConteudoPainelUpdateDto dados);
    Task<List<ConteudoPainelResponseDto>>
        GetConteudosDaTvAsync();

    Task DeleteAsync(Guid id);
}