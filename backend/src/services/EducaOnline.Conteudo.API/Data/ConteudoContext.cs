using EducaOnline.Conteudo.API.Models;
using EducaOnline.Core.Data;
using EducaOnline.Core.Messages;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Conteudo.API.Data
{
    public class ConteudoContext : DbContext, IUnitOfWork
    {
        public ConteudoContext(DbContextOptions<ConteudoContext> options)
            : base(options) { }


        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aula { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConteudoContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
