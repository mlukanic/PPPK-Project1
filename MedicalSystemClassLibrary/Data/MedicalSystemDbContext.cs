using MedicalSystemClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemClassLibrary.Data
{
    public class MedicalSystemDbContext : DbContext
    {
        public MedicalSystemDbContext() { }

        public MedicalSystemDbContext(DbContextOptions<MedicalSystemDbContext> options) : base(options) { }

        public const string CONNECTION_STRING = @"
            Host=touchily-obtainable-mammal.data-1.euc1.tembo.io;
            Port=5432;
            Username=postgres;
            Password=2LLh3HH0KFEoobKj;
            Database=medicinski_sustav
        ";

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<MedicalFile> MedicalFiles { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(CONNECTION_STRING)
                              .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Examination>()
                .HasOne(e => e.Patient)
                .WithMany(p => p.Examinations)
                .HasForeignKey(e => e.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId);

            modelBuilder.Entity<MedicalFile>()
                .HasOne(mf => mf.Examination)
                .WithMany(e => e.MedicalFiles)
                .HasForeignKey(mf => mf.ExaminationId);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientId);
        }
    }
}
