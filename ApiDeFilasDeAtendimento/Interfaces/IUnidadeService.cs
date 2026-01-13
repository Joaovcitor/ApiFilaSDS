using ApiDeFilasDeAtendimento.DTOs.Unidades;
using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IUnidadeService
    {
        Task<Unidade> Create(UnidadeDtoCreate dados);
        Task<List<Unidade>> GetUnidadesDoDonoLogado();
        Task<Unidade> GetById(Guid id);
    }
}
