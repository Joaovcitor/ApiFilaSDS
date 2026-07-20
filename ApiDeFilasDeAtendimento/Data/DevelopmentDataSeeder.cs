using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.Models;
using ApiDeFilasDeAtendimento.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiDeFilasDeAtendimento.Data;

public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<ApplicationRole>>();

        var settings = scope.ServiceProvider
            .GetRequiredService<IOptions<DevelopmentSeedSettings>>()
            .Value;

        ValidarConfiguracoes(settings);

        await context.Database.MigrateAsync();

        await CriarRolesAsync(roleManager);

        var admin = await CriarAdminAsync(
            userManager,
            settings);

        var unidade = await CriarUnidadeAsync(
            context,
            admin);

        await CriarUsuarioTvAsync(
            userManager,
            settings,
            unidade);
    }

    private static async Task CriarRolesAsync(
        RoleManager<ApplicationRole> roleManager)
    {
        string[] roles =
        [
            "Admin",
            "Atendente",
            "Totem",
            "SuperAdmin",
            "Tv"
        ];

        foreach (var roleName in roles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var resultado = await roleManager.CreateAsync(
                new ApplicationRole(roleName));

            ValidarResultadoIdentity(
                resultado,
                $"criar a role '{roleName}'");
        }
    }

    private static async Task<ApplicationUser> CriarAdminAsync(
        UserManager<ApplicationUser> userManager,
        DevelopmentSeedSettings settings)
    {
        var admin = await userManager.FindByEmailAsync(
            settings.AdminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = settings.AdminEmail,
                Email = settings.AdminEmail,
                EmailConfirmed = true,
                NomeCompleto = "Administrador de Desenvolvimento"
            };

            var resultadoCriacao = await userManager.CreateAsync(
                admin,
                settings.AdminPassword);

            ValidarResultadoIdentity(
                resultadoCriacao,
                "criar o administrador de desenvolvimento");
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            var resultadoRole = await userManager.AddToRoleAsync(
                admin,
                "Admin");

            ValidarResultadoIdentity(
                resultadoRole,
                "adicionar o usuário à role Admin");
        }

        return admin;
    }

    private static async Task<Unidade> CriarUnidadeAsync(
        AppDbContext context,
        ApplicationUser admin)
    {
        const string codigo = "UNIDADE-DEV-001";

        var unidade = await context.Unidade
            .FirstOrDefaultAsync(x =>
                x.Codigo == codigo &&
                x.DonoId == admin.Id);

        if (unidade is not null)
        {
            return unidade;
        }

        unidade = new Unidade
        {
            Id = Guid.NewGuid(),
            Local = "Unidade de Desenvolvimento",
            Codigo = codigo,
            Ativo = true,
            DonoId = admin.Id
        };

        context.Unidade.Add(unidade);
        await context.SaveChangesAsync();

        return unidade;
    }

    private static async Task<ApplicationUser> CriarUsuarioTvAsync(
        UserManager<ApplicationUser> userManager,
        DevelopmentSeedSettings settings,
        Unidade unidade)
    {
        var usuarioTv = await userManager.FindByEmailAsync(
            settings.TvEmail);

        if (usuarioTv is null)
        {
            usuarioTv = new ApplicationUser
            {
                UserName = settings.TvEmail,
                Email = settings.TvEmail,
                EmailConfirmed = true,
                NomeCompleto = "Televisão de Desenvolvimento",

                // A FK da unidade no seu usuário é LocalId
                LocalId = unidade.Id
            };

            var resultadoCriacao = await userManager.CreateAsync(
                usuarioTv,
                settings.TvPassword);

            ValidarResultadoIdentity(
                resultadoCriacao,
                "criar o usuário da televisão");
        }
        else if (usuarioTv.LocalId != unidade.Id)
        {
            usuarioTv.LocalId = unidade.Id;

            var resultadoAtualizacao =
                await userManager.UpdateAsync(usuarioTv);

            ValidarResultadoIdentity(
                resultadoAtualizacao,
                "vincular a televisão à unidade");
        }

        if (!await userManager.IsInRoleAsync(usuarioTv, "Tv"))
        {
            var resultadoRole = await userManager.AddToRoleAsync(
                usuarioTv,
                "Tv");

            ValidarResultadoIdentity(
                resultadoRole,
                "adicionar o usuário à role Tv");
        }

        return usuarioTv;
    }

    private static void ValidarConfiguracoes(
        DevelopmentSeedSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.AdminEmail))
        {
            throw new InvalidOperationException(
                "DevelopmentSeed:AdminEmail não foi configurado.");
        }

        if (string.IsNullOrWhiteSpace(settings.AdminPassword))
        {
            throw new InvalidOperationException(
                "DevelopmentSeed:AdminPassword não foi configurado.");
        }

        if (string.IsNullOrWhiteSpace(settings.TvEmail))
        {
            throw new InvalidOperationException(
                "DevelopmentSeed:TvEmail não foi configurado.");
        }

        if (string.IsNullOrWhiteSpace(settings.TvPassword))
        {
            throw new InvalidOperationException(
                "DevelopmentSeed:TvPassword não foi configurado.");
        }
    }

    private static void ValidarResultadoIdentity(
        IdentityResult resultado,
        string operacao)
    {
        if (resultado.Succeeded)
        {
            return;
        }

        var erros = string.Join(
            "; ",
            resultado.Errors.Select(x =>
                $"{x.Code}: {x.Description}"));

        throw new InvalidOperationException(
            $"Não foi possível {operacao}. Erros: {erros}");
    }
}