using EducaOnline.Aluno.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaOnline.Aluno.API.Data.Mappings
{
    public class AlunoMapping : IEntityTypeConfiguration<Models.Aluno>
    {
        public void Configure(EntityTypeBuilder<Models.Aluno> builder)
        {
            builder.ToTable("Alunos");

            builder.HasKey(a => a.Id);

            builder.OwnsOne(p => p.HistoricoAprendizado);

            builder.HasMany(a => a.Matriculas)
                   .WithOne(m => m.Aluno)
                   .HasForeignKey(m => m.AlunoId);

            builder.HasMany(a => a.Certificados)
                   .WithOne()
                   .HasForeignKey("AlunoId");

            builder.HasMany(a => a.AulasConcluidas)
                   .WithOne(a => a.Aluno)
                   .HasForeignKey(a => a.AlunoId);
        }
    }

    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matriculas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                   .IsRequired();

            builder.HasOne(m => m.Aluno)
                   .WithMany(a => a.Matriculas)
                   .HasForeignKey(m => m.AlunoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.ToTable("Certificados");

            builder.HasKey(x => x.Id);
        }
    }

    public class AulaConcluidaMapping : IEntityTypeConfiguration<AulaConcluida>
    {
        public void Configure(EntityTypeBuilder<AulaConcluida> builder)
        {
            builder.ToTable("AulaConcluidas");

            builder.HasKey(x => x.Id);
        }
    }
}
