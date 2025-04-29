using HemoVida.Core.Entities;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HemoVida.Infrastructure.Repositories;

public class DonorRepository : IDonorRepository
{
    private readonly HemoVidaDbContext _context;

    public DonorRepository(HemoVidaDbContext context)
    {
        _context = context;
    }

    public async Task<Donor> CreateDonor(Donor donor)
    {
        await _context.Donors.AddAsync(donor);
        await _context.SaveChangesAsync();
        return donor;
    }

    public async Task<Donor> GetByEmailAsync(string email)
    {
        var donor = await _context.Donors
        .Include(d => d.Address)
        .Include(d => d.User)
        .Include(d => d.Donations)
        .FirstOrDefaultAsync(d => d.User.Email == email);

        return donor;
    }

    public async Task UpdateDonor(Donor donor, int id)
    {
        var existingDonor = await _context.Donors
            .Include(d => d.Address)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (existingDonor == null)
            throw new Exception("Doador não encontrado.");

        existingDonor.BirthDate = donor.BirthDate;
        existingDonor.Weight = donor.Weight;
        existingDonor.BloodType = donor.BloodType;
        existingDonor.RhFactor = donor.RhFactor;
        existingDonor.Gender = donor.Gender;

        if (existingDonor.Address == null)
        {
            existingDonor.Address = donor.Address;
            existingDonor.Address.DonorId = existingDonor.Id;
        }
        else
        {
            existingDonor.Address.Street = donor.Address.Street;
            existingDonor.Address.City = donor.Address.City;
            existingDonor.Address.State = donor.Address.State;
            existingDonor.Address.ZipCode = donor.Address.ZipCode;
        }

        _context.Donors.Update(existingDonor);
        await _context.SaveChangesAsync();
    }
}
