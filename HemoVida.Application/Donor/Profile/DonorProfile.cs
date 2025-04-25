using HemoVida.Application.Donor.Response;

namespace HemoVida.Application.Donor.Profile;

public class DonorProfile : AutoMapper.Profile
{
    public DonorProfile()
    {
        CreateMap<Core.Entities.Donor, GetAvailableDonorsResponse>()
            .AfterMap((src, dest) => dest.Name = src.User.Name);

        CreateMap<List<GetAvailableDonorsResponse>, List<Core.Entities.Donor>>()
            .ConvertUsing((src, dest, context) =>
                src.Select(item => context.Mapper.Map<Core.Entities.Donor>(item)).ToList());

    }
}
