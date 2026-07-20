using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.DTOs.ConteudosPainel;
using ApiDeFilasDeAtendimento.Enums;
using ApiDeFilasDeAtendimento.Exceptions;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Services;

public class ConteudoPainelService : IConteudoPainelService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _environment;

    private const long TamanhoMaximoArquivo = 100 * 1024 * 1024;

    public ConteudoPainelService(
        AppDbContext context,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment environment)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _environment = environment;
    }

    public async Task<ConteudoPainelResponseDto> CreateAsync(
        ConteudoPainelCreateDto dados)
    {
        var usuarioLogado = await ObterUsuarioLogadoAsync();

        await ValidarUnidadeDoUsuarioAsync(
            dados.UnidadeId,
            usuarioLogado.Id);

        ValidarPeriodo(
            dados.InicioExibicao,
            dados.FimExibicao);

        ValidarArquivo(
            dados.Arquivo,
            dados.TipoConteudo);

        var conteudo = _mapper.Map<ConteudoPainel>(dados);

        var arquivoSalvo = await SalvarArquivoAsync(
            dados.Arquivo,
            dados.UnidadeId);

        conteudo.Id = Guid.NewGuid();
        conteudo.CaminhoArquivo = arquivoSalvo.CaminhoRelativo;
        conteudo.NomeArquivoOriginal = dados.Arquivo.FileName;
        conteudo.NomeArquivoArmazenado = arquivoSalvo.NomeArmazenado;
        conteudo.ContentType = dados.Arquivo.ContentType;
        conteudo.TamanhoBytes = dados.Arquivo.Length;
        conteudo.CriadoEm = DateTime.UtcNow;

        _context.Set<ConteudoPainel>().Add(conteudo);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            ExcluirArquivoFisico(conteudo.CaminhoArquivo);
            throw;
        }

        await _context.Entry(conteudo)
            .Reference(x => x.Unidade)
            .LoadAsync();

        return _mapper.Map<ConteudoPainelResponseDto>(conteudo);
    }

    public async Task<List<ConteudoPainelResponseDto>> GetAllAsync()
    {
        var usuarioLogado = await ObterUsuarioLogadoAsync();

        var conteudos = await _context.Set<ConteudoPainel>()
            .AsNoTracking()
            .Include(x => x.Unidade)
            .Where(x => x.Unidade.DonoId == usuarioLogado.Id)
            .OrderBy(x => x.Unidade.Local)
            .ThenBy(x => x.OrdemExibicao)
            .ThenBy(x => x.CriadoEm)
            .ToListAsync();

        return _mapper.Map<List<ConteudoPainelResponseDto>>(
            conteudos);
    }

    public async Task<List<ConteudoPainelResponseDto>>
        GetByUnidadeAsync(Guid unidadeId)
    {
        var usuarioLogado = await ObterUsuarioLogadoAsync();

        await ValidarUnidadeDoUsuarioAsync(
            unidadeId,
            usuarioLogado.Id);

        var conteudos = await _context.Set<ConteudoPainel>()
            .AsNoTracking()
            .Include(x => x.Unidade)
            .Where(x => x.UnidadeId == unidadeId)
            .OrderBy(x => x.OrdemExibicao)
            .ThenBy(x => x.CriadoEm)
            .ToListAsync();

        return _mapper.Map<List<ConteudoPainelResponseDto>>(
            conteudos);
    }

    public async Task<ConteudoPainelResponseDto> GetByIdAsync(
        Guid id)
    {
        var usuarioLogado = await ObterUsuarioLogadoAsync();

        var conteudo = await _context.Set<ConteudoPainel>()
            .AsNoTracking()
            .Include(x => x.Unidade)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Unidade.DonoId == usuarioLogado.Id)
            ?? throw new NotFoundException(
                "Conteúdo do painel não encontrado");

        return _mapper.Map<ConteudoPainelResponseDto>(conteudo);
    }
    
    public async Task<List<ConteudoPainelResponseDto>>
        GetConteudosDaTvAsync()
    {
        var usuarioLogado = await ObterUsuarioLogadoAsync();

        var agora = DateTime.UtcNow;

        var conteudos = await _context.Set<ConteudoPainel>()
            .AsNoTracking()
            .Include(x => x.Unidade)
            .Where(x =>
                x.UnidadeId == usuarioLogado.LocalId &&
                x.Ativo &&
                (!x.InicioExibicao.HasValue ||
                 x.InicioExibicao.Value <= agora) &&
                (!x.FimExibicao.HasValue ||
                 x.FimExibicao.Value >= agora))
            .OrderBy(x => x.OrdemExibicao)
            .ThenBy(x => x.CriadoEm)
            .ToListAsync();

        return _mapper.Map<List<ConteudoPainelResponseDto>>(
            conteudos);
    }

    public async Task<ConteudoPainelResponseDto> UpdateAsync(
        Guid id,
        ConteudoPainelUpdateDto dados)
    {
        var usuarioLogado = await ObterUsuarioLogadoAsync();

        var conteudo = await _context.Set<ConteudoPainel>()
            .Include(x => x.Unidade)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Unidade.DonoId == usuarioLogado.Id)
            ?? throw new NotFoundException(
                "Conteúdo do painel não encontrado");

        ValidarPeriodo(
            dados.InicioExibicao,
            dados.FimExibicao);

        var caminhoArquivoAnterior = conteudo.CaminhoArquivo;

        _mapper.Map(dados, conteudo);

        if (dados.Arquivo is not null)
        {
            ValidarArquivo(
                dados.Arquivo,
                dados.TipoConteudo);

            var arquivoSalvo = await SalvarArquivoAsync(
                dados.Arquivo,
                conteudo.UnidadeId);

            conteudo.CaminhoArquivo = arquivoSalvo.CaminhoRelativo;
            conteudo.NomeArquivoOriginal = dados.Arquivo.FileName;
            conteudo.NomeArquivoArmazenado =
                arquivoSalvo.NomeArmazenado;
            conteudo.ContentType = dados.Arquivo.ContentType;
            conteudo.TamanhoBytes = dados.Arquivo.Length;
        }

        conteudo.AtualizadoEm = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        if (dados.Arquivo is not null)
        {
            ExcluirArquivoFisico(caminhoArquivoAnterior);
        }

        return _mapper.Map<ConteudoPainelResponseDto>(conteudo);
    }

    public async Task DeleteAsync(Guid id)
    {
        var usuarioLogado = await ObterUsuarioLogadoAsync();

        var conteudo = await _context.Set<ConteudoPainel>()
            .Include(x => x.Unidade)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Unidade.DonoId == usuarioLogado.Id)
            ?? throw new NotFoundException(
                "Conteúdo do painel não encontrado");

        var caminhoArquivo = conteudo.CaminhoArquivo;

        _context.Set<ConteudoPainel>().Remove(conteudo);
        await _context.SaveChangesAsync();

        ExcluirArquivoFisico(caminhoArquivo);
    }

    private async Task<ApplicationUser> ObterUsuarioLogadoAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new UnauthorizedAccessException(
                "Contexto HTTP não disponível");

        return await _userManager.GetUserAsync(httpContext.User)
            ?? throw new UnauthorizedAccessException(
                "Você deve fazer login");
    }

    private async Task<Unidade> ValidarUnidadeDoUsuarioAsync(
        Guid unidadeId,
        string usuarioId)
    {
        return await _context.Unidade
            .FirstOrDefaultAsync(x =>
                x.Id == unidadeId &&
                x.DonoId == usuarioId)
            ?? throw new NotFoundException(
                "Unidade não encontrada");
    }

    private static void ValidarPeriodo(
        DateTime? inicio,
        DateTime? fim)
    {
        if (inicio.HasValue &&
            fim.HasValue &&
            fim.Value <= inicio.Value)
        {
            throw new BadRequestException(
                "A data final deve ser posterior à data inicial");
        }
    }

    private static void ValidarArquivo(
        IFormFile arquivo,
        TipoConteudoPainel tipoConteudo)
    {
        if (arquivo.Length == 0)
        {
            throw new BadRequestException(
                "O arquivo enviado está vazio");
        }

        if (arquivo.Length > TamanhoMaximoArquivo)
        {
            throw new BadRequestException(
                "O arquivo não pode ultrapassar 100 MB");
        }

        var extensao = Path.GetExtension(arquivo.FileName)
            .ToLowerInvariant();

        var extensoesImagem = new[]
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        var extensoesVideo = new[]
        {
            ".mp4", ".webm"
        };

        var extensaoValida = tipoConteudo switch
        {
            TipoConteudoPainel.Imagem =>
                extensoesImagem.Contains(extensao),

            TipoConteudoPainel.Video =>
                extensoesVideo.Contains(extensao),

            _ => false
        };

        if (!extensaoValida)
        {
            throw new BadRequestException(
                "O formato do arquivo não corresponde ao tipo de conteúdo");
        }
    }

    private async Task<ArquivoSalvo> SalvarArquivoAsync(
        IFormFile arquivo,
        Guid unidadeId)
    {
        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot");
        }

        var pastaRelativa = Path.Combine(
            "uploads",
            "painel",
            unidadeId.ToString());

        var pastaCompleta = Path.Combine(
            webRootPath,
            pastaRelativa);

        Directory.CreateDirectory(pastaCompleta);

        var extensao = Path.GetExtension(arquivo.FileName)
            .ToLowerInvariant();

        var nomeArmazenado =
            $"{Guid.NewGuid():N}{extensao}";

        var caminhoCompleto = Path.Combine(
            pastaCompleta,
            nomeArmazenado);

        await using var stream = new FileStream(
            caminhoCompleto,
            FileMode.CreateNew);

        await arquivo.CopyToAsync(stream);

        var caminhoRelativo =
            $"/uploads/painel/{unidadeId}/{nomeArmazenado}";

        return new ArquivoSalvo(
            nomeArmazenado,
            caminhoRelativo);
    }

    private void ExcluirArquivoFisico(string? caminhoRelativo)
    {
        if (string.IsNullOrWhiteSpace(caminhoRelativo))
        {
            return;
        }

        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot");
        }

        var caminhoNormalizado = caminhoRelativo
            .TrimStart('/')
            .Replace('/', Path.DirectorySeparatorChar);

        var caminhoCompleto = Path.Combine(
            webRootPath,
            caminhoNormalizado);

        if (File.Exists(caminhoCompleto))
        {
            File.Delete(caminhoCompleto);
        }
    }

    private sealed record ArquivoSalvo(
        string NomeArmazenado,
        string CaminhoRelativo);
}