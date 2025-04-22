using HemoVida.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoVida.Infrastructure.Persistence.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.BloodType).IsRequired().HasMaxLength(3);
        builder.Property(s => s.RhFactor).IsRequired();
        builder.Property(s => s.MlQuantity).IsRequired();
    }
}