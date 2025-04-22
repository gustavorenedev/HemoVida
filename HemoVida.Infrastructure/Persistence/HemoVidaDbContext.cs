using HemoVida.Core.Entities;
using HemoVida.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HemoVida.Infrastructure.Persistence;

public class HemoVidaDbContext : DbContext
{
    public HemoVidaDbContext(DbContextOptions<HemoVidaDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<Donor> Donors { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new DonationConfiguration());
        modelBuilder.ApplyConfiguration(new DonorConfiguration());
        modelBuilder.ApplyConfiguration(new StockConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
