using HemoVida.Application.Donation.Request;

namespace HemoVida.Application.Donation.Profile;

public class DonationProfile : AutoMapper.Profile
{
    public DonationProfile()
    {
        CreateMap<Core.Entities.Donation, DonationPublisherRequest>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Donor.User.Name))
            .ForMember(dest => dest.MlQuantity, opt => opt.MapFrom(src => src.MlQuantity))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Donor.User.Email))
            .ForMember(dest => dest.DonationDate, opt => opt.MapFrom(src => src.DonationDate))
            .ForMember(dest => dest.RhFactor, opt => opt.MapFrom(src => src.Donor.RhFactor))
            .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => src.Donor.BloodType));
    }
}
