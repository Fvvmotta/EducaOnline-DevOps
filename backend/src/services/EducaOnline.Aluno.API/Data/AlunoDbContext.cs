using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Data;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Aluno.API.Data
{
    public class AlunoDbContext : DbContext, IUnitOfWork
    {
        public AlunoDbContext(DbContextOptions<AlunoDbContext> options)
            : base(options) { }

        public DbSet<Models.Aluno> Alunos { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<AulaConcluida> AulasConcluidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoDbContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
