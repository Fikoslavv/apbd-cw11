using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apbd_cw11.Database;

public class PrescribedMedicineConfiguration : IEntityTypeConfiguration<PrescribedMedicine>
{
    public void Configure(EntityTypeBuilder<PrescribedMedicine> builder)
    {
        builder.ToTable("Prescription_Medicament");

        builder.HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

        builder.Property(pm => pm.Dose);

        builder.Property(pm => pm.Details).HasMaxLength(100);

        builder.HasOne(med => med.Prescription).WithMany(pre => pre.PrescribedMedicine).HasForeignKey(med => med.IdPrescription).IsRequired();

        builder.HasOne(med => med.Medicament).WithMany(med => med.PrescribedMedicine).HasForeignKey(med => med.IdMedicament).IsRequired();
    }
}
