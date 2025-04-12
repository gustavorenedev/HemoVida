using HemoVida.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoVida.Infrastructure.Persistence.Configurations;

public class DonorConfiguration : IEntityTypeConfiguration<Donor>
{
    public void Configure(EntityTypeBuilder<Donor> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.BirthDate).IsRequired();
        builder.Property(d => d.Weight).IsRequired();
        builder.Property(d => d.BloodType).IsRequired().HasMaxLength(3);
        builder.Property(d => d.RhFactor).IsRequired();

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}