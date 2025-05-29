using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apbd_cw11.Database;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable(nameof(Prescription));

        builder.HasKey(pre => pre.IdPrescription);

        builder.Property(pre => pre.Date).IsRequired().HasColumnType("date");

        builder.Property(pre => pre.DueDate).IsRequired().HasColumnType("date");

        builder.HasOne(pre => pre.Patient).WithMany(pat => pat.Prescriptions).HasForeignKey(pre => pre.IdPatient).IsRequired();

        builder.HasOne(pre => pre.Doctor).WithMany(doc => doc.Prescriptions).HasForeignKey(pre => pre.IdDoctor).IsRequired();

        builder.HasMany(pre => pre.PrescribedMedicine).WithOne(med => med.Prescription).HasForeignKey(med => med.IdPrescription);
    }
}
