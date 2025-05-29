using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apbd_cw11;

public class MedicamentConfiguration : IEntityTypeConfiguration<Medicament>
{
    public void Configure(EntityTypeBuilder<Medicament> builder)
    {
        builder.ToTable(nameof(Medicament));

        builder.HasKey(med => med.IdMedicament);

        builder.Property(med => med.Name).IsRequired().HasMaxLength(100);

        builder.Property(med => med.Description).IsRequired().HasMaxLength(100);

        builder.Property(med => med.Type).IsRequired().HasMaxLength(100);

        builder.HasMany(med => med.PrescribedMedicine).WithOne(med => med.Medicament).HasForeignKey(med => med.IdMedicament);
    }
}
