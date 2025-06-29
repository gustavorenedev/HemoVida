﻿namespace HemoVida.Application.Donation.Request;

public class DonationPublisherRequest
{
    public string Name { get; set; }
    public DateTime DonationDate { get; set; }
    public string Email { get; set; }
    public string BloodType { get; set; }
    public int MlQuantity { get; set; }
    public string RhFactor { get; set; }
}