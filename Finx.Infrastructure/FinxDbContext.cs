using Finx.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finx.Infrastructure
{
    public class FinxDbContext : DbContext
    {
        public FinxDbContext(DbContextOptions<FinxDbContext> options) : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<HistoricoMedico> HistoricosMedicos { get; set; }
        public DbSet<Hospital> Hospitais { get; set; }
        public DbSet<PacienteHospital> PacienteHospitais { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.Cpf);
                b.Property(x => x.Nome).IsRequired();
                b.Property(x => x.Cpf).IsRequired();
            });

            modelBuilder.Entity<HistoricoMedico>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.PacienteId);
                b.Property(x => x.Diagnostico);
            });

            modelBuilder.Entity<Hospital>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired();
            });

            modelBuilder.Entity<PacienteHospital>(b =>
            {
                b.HasKey(x => new { x.PacienteId, x.HospitalId });
                b.HasIndex(x => new { x.HospitalId, x.Codigo });
                b.Property(x => x.Codigo).IsRequired();
            });

            modelBuilder.Entity<Agendamento>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => new { x.HospitalId, x.PacienteId });
            });
        }
    }
}
