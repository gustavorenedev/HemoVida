using HemoVida.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoVida.Infrastructure.Persistence.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasData(
            new Stock { Id = 1, BloodType = "A", RhFactor = "+", MlQuantity = 450 },
            new Stock { Id = 2, BloodType = "A", RhFactor = "-", MlQuantity = 470 },
            new Stock { Id = 3, BloodType = "B", RhFactor = "+", MlQuantity = 420 },
            new Stock { Id = 4, BloodType = "B", RhFactor = "-", MlQuantity = 430 },
            new Stock { Id = 5, BloodType = "AB", RhFactor = "+", MlQuantity = 460 },
            new Stock { Id = 6, BloodType = "AB", RhFactor = "-", MlQuantity = 440 },
            new Stock { Id = 7, BloodType = "O", RhFactor = "+", MlQuantity = 425 },
            new Stock { Id = 8, BloodType = "O", RhFactor = "-", MlQuantity = 465 },
            new Stock { Id = 9, BloodType = "A", RhFactor = "+", MlQuantity = 455 },
            new Stock { Id = 10, BloodType = "B", RhFactor = "+", MlQuantity = 470 },
            new Stock { Id = 11, BloodType = "O", RhFactor = "-", MlQuantity = 420 },
            new Stock { Id = 12, BloodType = "AB", RhFactor = "+", MlQuantity = 435 },
            new Stock { Id = 13, BloodType = "A", RhFactor = "-", MlQuantity = 445 },
            new Stock { Id = 14, BloodType = "B", RhFactor = "-", MlQuantity = 428 },
            new Stock { Id = 15, BloodType = "O", RhFactor = "+", MlQuantity = 438 }
            );
        builder.HasKey(s => s.Id);

        builder.Property(s => s.BloodType).IsRequired().HasMaxLength(3);
        builder.Property(s => s.RhFactor).IsRequired();
        builder.Property(s => s.MlQuantity).IsRequired();
    }
}