using HemoVida.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoVida.Infrastructure.Persistence.Configurations;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DonationDate).IsRequired();

        builder.HasOne(d => d.Donor)
            .WithMany(donor => donor.Donations)
            .HasForeignKey(d => d.DonorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}