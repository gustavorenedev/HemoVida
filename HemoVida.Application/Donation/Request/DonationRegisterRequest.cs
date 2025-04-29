using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HemoVida.Application.Donation.Request;

public class DonationRegisterRequest
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Quantidade de sangue é obrigatório")]
    public int MlQuantity { get; set; }
    [JsonIgnore]
    public DateTime DonationDate { get; set; } = DateTime.Now;
}

public class DonationRegisterRequestValidator : AbstractValidator<DonationRegisterRequest>
{
    public DonationRegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email do doador é obrigatório.");
        RuleFor(x => x.MlQuantity)
            .NotEmpty()
            .WithMessage("A quantidade de mililitros é obrigatória.")
            .InclusiveBetween(420, 470)
            .WithMessage("A quantidade de mililitros deve ser entre 420ml e 470ml.");
    }
}
