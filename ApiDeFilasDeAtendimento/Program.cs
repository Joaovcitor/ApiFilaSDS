using ApiDeFilasDeAtendimento.Context;
using ApiDeFilasDeAtendimento.Handlers;
using ApiDeFilasDeAtendimento.Hubs;
using ApiDeFilasDeAtendimento.Interfaces;
using ApiDeFilasDeAtendimento.Models;
using ApiDeFilasDeAtendimento.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity; // Adicionado
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "FilaAuth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;

    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    options.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Registro de Services
builder.Services.AddScoped<IFilaSenhaService, FilaSenhaService>();
builder.Services.AddScoped<IGuicheService, GuicheService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUnidadeService, UnidadeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhasPoliticas", policy =>
    {
        policy.WithOrigins("https://fila-sds.socialquixada.com.br", "http://localhost:8080") // apenas produção
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials() // ESSENCIAL para cookies
              .WithExposedHeaders("X-Pagination");
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// service de roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AcessoAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AcessoOperacional", policy => policy.RequireRole("Atendente"));
    options.AddPolicy("AcessoTotem", policy => policy.RequireRole("Totem"));
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var roles = new[] { "Admin", "Atendente", "Totem", "SuperAdmin" };
    foreach (var role in roles)
    {
        if (!await rolesManager.RoleExistsAsync(role))
        {
            await rolesManager.CreateAsync(new ApplicationRole(role));
        }
    }
}

app.UseExceptionHandler();

app.UseCors("MinhasPoliticas");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();
app.MapControllers();
app.MapHub<QueueHub>("/hubs/queue");

app.Run();
