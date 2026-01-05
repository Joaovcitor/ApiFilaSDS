using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<FilaSenha> FilaSenha {  get; set; }
        public DbSet<Unidade> Unidade { get; set; }
        public DbSet<Guiche> Guiche { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
