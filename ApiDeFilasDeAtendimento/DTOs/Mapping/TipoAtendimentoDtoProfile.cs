using ApiDeFilasDeAtendimento.DTOs.Atendimentos;
using ApiDeFilasDeAtendimento.Enums;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;

namespace ApiDeFilasDeAtendimento.DTOs.Mapping
{
    public class TipoAtendimentoDtoProfile : Profile
    {
        public TipoAtendimentoDtoProfile()
        {
            CreateMap<TiposDeAtendimento, TipoDeAtendimentoDtoCreate>().ReverseMap();
            CreateMap<TiposDeAtendimento, TipoAtendimentoDtoUpdate>().ReverseMap();
            CreateMap<TiposDeAtendimento, TipoAtendimentoDtoResponse>().ReverseMap();
        }
    }
}
