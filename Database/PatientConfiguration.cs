using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apbd_cw11.Database;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable(nameof(Patient));

        builder.HasKey(pat => pat.IdPatient);

        builder.Property(pat => pat.FirstName).IsRequired().HasMaxLength(100);

        builder.Property(pat => pat.LastName).IsRequired().HasMaxLength(100);

        builder.Property(pat => pat.BirthDate).IsRequired().HasColumnType("date");

        builder.HasMany(pat => pat.Prescriptions).WithOne(pre => pre.Patient).HasForeignKey(pre => pre.IdPatient);
    }
}
