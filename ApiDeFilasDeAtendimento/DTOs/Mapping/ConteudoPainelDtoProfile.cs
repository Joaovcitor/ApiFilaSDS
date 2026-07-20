using ApiDeFilasDeAtendimento.DTOs.ConteudosPainel;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;

namespace ApiDeFilasDeAtendimento.DTOs.Mapping;


public class ConteudoPainelProfile : Profile
{
    public ConteudoPainelProfile()
    {
        CreateMap<ConteudoPainelCreateDto, ConteudoPainel>()
            .ForMember(
                destino => destino.Id,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.CaminhoArquivo,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.NomeArquivoOriginal,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.NomeArquivoArmazenado,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.ContentType,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.TamanhoBytes,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.Unidade,
                opcoes => opcoes.Ignore());

        CreateMap<ConteudoPainelUpdateDto, ConteudoPainel>()
            .ForMember(
                destino => destino.Id,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.CaminhoArquivo,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.NomeArquivoOriginal,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.NomeArquivoArmazenado,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.ContentType,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.TamanhoBytes,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.UnidadeId,
                opcoes => opcoes.Ignore())
            .ForMember(
                destino => destino.Unidade,
                opcoes => opcoes.Ignore());

        CreateMap<ConteudoPainel, ConteudoPainelResponseDto>()
            .ForMember(
                destino => destino.Unidade,
                opcoes => opcoes.MapFrom(origem => origem.Unidade.Local));
    }
}