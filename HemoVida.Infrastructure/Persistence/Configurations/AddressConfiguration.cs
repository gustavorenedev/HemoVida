using HemoVida.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoVida.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Street).IsRequired().HasMaxLength(100);
        builder.Property(a => a.City).IsRequired().HasMaxLength(50);
        builder.Property(a => a.State).IsRequired().HasMaxLength(50);
        builder.Property(a => a.ZipCode).IsRequired().HasMaxLength(10);

        builder.HasOne(a => a.Donor)
            .WithOne(d => d.Address)
            .HasForeignKey<Address>(a => a.DonorId);
    }
}
