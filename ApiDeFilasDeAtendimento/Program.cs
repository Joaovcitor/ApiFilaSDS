using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.Handlers;
using ApiDeFilasDeAtendimento.Hubs;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using ApiDeFilasDeAtendimento.Services;
using ApiDeFilasDeAtendimento.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using ApiDeFilasDeAtendimento.Data;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

const long tamanhoMaximoUpload = 100 * 1024 * 1024;

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(30);
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "FilaAuth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;

    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            return Task.CompletedTask;
        },

        OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode =
                StatusCodes.Status403Forbidden;

            return Task.CompletedTask;
        }
    };
});

// Limite para upload via multipart/form-data
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = tamanhoMaximoUpload;
});

// Limite do servidor Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = tamanhoMaximoUpload;
});

// Controllers
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());

        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// OpenAPI
builder.Services.AddOpenApi();

// AutoMapper
builder.Services.AddAutoMapper(
    configuration => { },
    Assembly.GetExecutingAssembly());

// SignalR
builder.Services
    .AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// Configurações
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Serviços
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.Configure<DevelopmentSeedSettings>(
    builder.Configuration.GetSection("DevelopmentSeed"));
builder.Services.AddScoped<IFilaSenhaService, FilaSenhaService>();
builder.Services.AddScoped<IGuicheService, GuicheService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUnidadeService, UnidadeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<
    ITipoAtendimentosService,
    TipoAtendimentoService>();

builder.Services.AddScoped<
    IConteudoPainelService,
    ConteudoPainelService>();

builder.Services.AddHttpContextAccessor();

// Tratamento de erros
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "AcessoAdmin",
        policy => policy.RequireRole("Admin", "SuperAdmin"));

    options.AddPolicy(
        "AcessoOperacional",
        policy => policy.RequireRole("Atendente"));

    options.AddPolicy(
        "AcessoTotem",
        policy => policy.RequireRole("Totem"));

    options.AddPolicy(
        "AcessoSuperAdmin",
        policy => policy.RequireRole("SuperAdmin"));

    options.AddPolicy(
        "AcessoTv",
        policy => policy.RequireRole("Tv"));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhasPoliticas", policy =>
    {
        policy
            .WithOrigins(
                "https://fila-sds.socialquixada.com.br",
                "http://localhost:8080",
                "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("X-Pagination");
    });
});

var app = builder.Build();

// Criação automática das roles
using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<ApplicationRole>>();

    string[] roles =
    [
        "Admin",
        "Atendente",
        "Totem",
        "SuperAdmin",
        "Tv"
    ];

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var resultado = await roleManager.CreateAsync(
                new ApplicationRole(role));

            if (!resultado.Succeeded)
            {
                var erros = string.Join(
                    "; ",
                    resultado.Errors.Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Não foi possível criar a role '{role}': {erros}");
            }
        }
    }
}

// Pipeline
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await DevelopmentDataSeeder.SeedAsync(
        app.Services);
}
app.UseHttpsRedirection();

var contentTypeProvider = new FileExtensionContentTypeProvider();

contentTypeProvider.Mappings[".mp4"] = "video/mp4";
contentTypeProvider.Mappings[".webm"] = "video/webm";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider,
    ServeUnknownFileTypes = false
});

app.UseCors("MinhasPoliticas");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();
app.MapControllers();
app.MapHub<QueueHub>("/hubs/queue");

app.Run();