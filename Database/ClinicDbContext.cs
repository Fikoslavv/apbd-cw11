using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Database;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescribedMedicine> PrescribedMedicine { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DoctorConfiguration());
        builder.ApplyConfiguration(new PatientConfiguration());
        builder.ApplyConfiguration(new MedicamentConfiguration());
        builder.ApplyConfiguration(new PrescriptionConfiguration());
        builder.ApplyConfiguration(new PrescribedMedicineConfiguration());
    }
}
