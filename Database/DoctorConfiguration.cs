using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apbd_cw11.Database;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable(nameof(Doctor));

        builder.HasKey(doc => doc.IdDoctor);

        builder.Property(doc => doc.FirstName).IsRequired().HasMaxLength(100);

        builder.Property(doc => doc.LastName).IsRequired().HasMaxLength(100);

        builder.Property(doc => doc.Email).IsRequired().HasMaxLength(100);

        builder.HasMany(doc => doc.Prescriptions).WithOne(pre => pre.Doctor).HasForeignKey(pre => pre.IdDoctor);
    }
}
