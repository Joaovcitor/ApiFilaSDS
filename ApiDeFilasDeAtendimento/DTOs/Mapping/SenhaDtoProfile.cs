using ApiDeFilasDeAtendimento.DTOs.Senhas;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;

namespace ApiDeFilasDeAtendimento.DTOs.Mapping
{
    public class SenhaDtoProfile : Profile
    {
        public SenhaDtoProfile()
        {
            CreateMap<FilaSenha, SenhaDtoCreate>().ReverseMap();
            CreateMap<FilaSenha, SenhaDtoUpdateStatusForCall>().ReverseMap();
            CreateMap<FilaSenha, SenhaDtoUpdateStatusForAtendimento>().ReverseMap();
            CreateMap<FilaSenha, SenhaDtoUpdateStatusForCancel>().ReverseMap();
            CreateMap<FilaSenha, SenhaDtoUpdateStatusForCall>().ReverseMap();
            CreateMap<FilaSenha, SenhaDtoUpdateMotivoAtendimento>().ReverseMap();
            CreateMap<FilaSenha, SenhaDtoUpdateNameEmployee>().ReverseMap();
        }
    }
}
