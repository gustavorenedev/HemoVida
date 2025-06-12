using HemoVida.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoVida.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User { Id = 1, Name = "Admin", Password = BCrypt.Net.BCrypt.HashPassword("Senha@123"), Email = "admin@hemovida.com", Role = Core.Enum.Role.Admin },
            new User { Id = 2, Name = "Gustavo", Password = BCrypt.Net.BCrypt.HashPassword("Senha@123"), Email = "gustavo@gmail.com", Role = Core.Enum.Role.User },
            new User { Id = 3, Name = "João", Password = BCrypt.Net.BCrypt.HashPassword("Senha@123"), Email = "joao@gmail.com", Role = Core.Enum.Role.User },
            new User { Id = 4, Name = "Renan", Password = BCrypt.Net.BCrypt.HashPassword("Senha@123"), Email = "renan@gmail.com", Role = Core.Enum.Role.User },
            new User { Id = 5, Name = "Eduardo", Password = BCrypt.Net.BCrypt.HashPassword("Senha@123"), Email = "eduardo@gmail.com", Role = Core.Enum.Role.User }
        );

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.Role).IsRequired();
    }
}