using EducaOnline.Conteudo.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducaOnline.Conteudo.API.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("Cursos");
                        
            builder.HasKey(x => x.Id);

            builder.OwnsOne(p => p.ConteudoProgramatico)
                .Property(p => p.Titulo);

            builder.OwnsOne(p => p.ConteudoProgramatico)
                .Property(p => p.Descricao);

            builder.OwnsOne(p => p.ConteudoProgramatico)
                .Property(p => p.Objetivos);

            builder.OwnsOne(p => p.ConteudoProgramatico)
                .Property(p => p.CargaHoraria);

            builder.HasMany(p => p.Aulas)
                .WithOne(p => p.Curso)
                .HasForeignKey(p => p.CursoId);

            builder.Property(p => p.Nome)
                .IsRequired();

            builder.Property(p => p.Valor)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
        }
    }

    public class AulaMapping : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            builder.ToTable("Aulas");

            builder.HasKey(x => x.Id);
        }
    }
}
