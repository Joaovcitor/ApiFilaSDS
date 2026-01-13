using ApiDeFilasDeAtendimento.DTOs.Unidades;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;

namespace ApiDeFilasDeAtendimento.DTOs.Mapping
{
    public class UnidadeDtoProfile : Profile
    {
        public UnidadeDtoProfile()
        {
            CreateMap<Unidade, UnidadeDtoCreate>().ReverseMap();
            CreateMap<Unidade, UnidadeDtoUpdate>().ReverseMap();
        }
    }
}
