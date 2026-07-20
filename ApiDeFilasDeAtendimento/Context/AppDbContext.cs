using ApiDeFilasDeAtendimento.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiDeFilasDeAtendimento.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<FilaSenha> FilaSenha { get; set; }
        public DbSet<Unidade> Unidade { get; set; }
        public DbSet<Guiche> Guiche { get; set; }
        public DbSet<TiposDeAtendimento> TiposAtendimento { get; set; }
        public DbSet<ConteudoPainel> ConteudoPainels { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
            .HasOne(u => u.Local)
            .WithMany(u => u.ApplicationUsers)
            .HasForeignKey(u => u.LocalId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
