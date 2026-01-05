using ApiDeFilasDeAtendimento.DTOs.Guiches;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;

namespace ApiDeFilasDeAtendimento.DTOs.Mapping
{
    public class GuicheDtoProfile : Profile
    {
        public GuicheDtoProfile()
        {
            CreateMap<Guiche, GuicheCreateDto>().ReverseMap();
        }
    }
}
