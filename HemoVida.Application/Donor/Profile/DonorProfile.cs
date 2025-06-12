using HemoVida.Application.Donor.Response;

namespace HemoVida.Application.Donor.Profile;

public class DonorProfile : AutoMapper.Profile
{
    public DonorProfile()
    {
        CreateMap<Core.Entities.Donor, GetAvailableDonorsResponse>()
            .AfterMap((src, dest) => {
                dest.Name = src.User.Name;
                dest.Email = src.User.Email;
            });

        CreateMap<List<GetAvailableDonorsResponse>, List<Core.Entities.Donor>>()
            .ConvertUsing((src, dest, context) =>
                src.Select(item => context.Mapper.Map<Core.Entities.Donor>(item)).ToList());

        CreateMap<Core.Entities.Donation, DonationsResponse>();

        CreateMap<Core.Entities.Donor, GetDonationHistoryResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Donations, opt => opt.MapFrom(src => src.Donations));
    }
}
