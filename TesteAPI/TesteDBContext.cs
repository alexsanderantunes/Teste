using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesteAPI.Models;

namespace TesteAPI
{
    public class TesteDBContext : DbContext
    {
        public TesteDBContext(): base("VisitanteDB") 
        {
            Database.SetInitializer<TesteDBContext>(new CreateDatabaseIfNotExists<TesteDBContext>());
        }

        public DbSet<Visitante> Visitantes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Fluent Configurações
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<Visitante>().ToTable("Visitante");

            modelBuilder.Entity<Visitante>().HasKey<int>(s => s.Id);

            modelBuilder.Entity<Visitante>()
                .Property(p => p.Nome)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Visitante>()
                .Property(p => p.Mensagem)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<Visitante>()
                .Property(p => p.Endereco)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Visitante>()
                .Property(p => p.Localizacao)
                .IsOptional();

            //base.OnModelCreating(modelBuilder);
        }
    }
}