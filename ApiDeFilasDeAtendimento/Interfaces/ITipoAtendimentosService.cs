using ApiDeFilasDeAtendimento.DTOs.Atendimentos;
using ApiDeFilasDeAtendimento.Enums;
using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface ITipoAtendimentosService
    {
        Task<TiposDeAtendimento> Create(TipoDeAtendimentoDtoCreate dados);
        Task<List<TipoAtendimentoDtoResponse>> GetAll();
        Task<TiposDeAtendimento> GetById(Guid id);
        Task<TipoAtendimentoDtoResponse> Update(Guid Id, TipoAtendimentoDtoUpdate dados);
    }
}
