using ApiDeFilasDeAtendimento.DTOs.Guiches;
using ApiDeFilasDeAtendimento.Models;

namespace ApiDeFilasDeAtendimento.Interfaces
{
    public interface IGuicheService
    {
        Task<Guiche> CreateGuiche(GuicheCreateDto dados);
        Task<Guiche> GuicheDoUsuario();
    }
}
