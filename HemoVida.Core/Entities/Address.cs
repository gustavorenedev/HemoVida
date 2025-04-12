namespace HemoVida.Core.Entities;

public class Address : BaseEntity
{
    public Address(Donor donor, string street, string city, string state, string zipCode)
    {
        Donor = donor;
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public Donor Donor { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
