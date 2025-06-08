using FluentValidation;
using HemoVida.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace HemoVida.Application.Donor.Request;

public class CreateDonorRequest
{
    [Required(ErrorMessage = "Email é obrigatório")]
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public double Weight { get; set; }
    [Required(ErrorMessage = "Tipo Sanguíneo é obrigatório")]
    public string BloodType { get; set; }
    [Required(ErrorMessage = "Fator RH é obrigatório")]
    public string RhFactor { get; set; }
    [Required(ErrorMessage = "Postal Code é obrigatório")]
    public string ZipCode { get; set; }
    public Gender Gender { get; set; }
}

public class CreateDonorRequestValidator : AbstractValidator<CreateDonorRequest>
{
    public CreateDonorRequestValidator()
    {
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Data de nascimento é obrigatória.")
            .LessThan(DateTime.Now)
            .WithMessage("A data de nascimento deve ser no passado.")
            .Must(birthDate => birthDate <= DateTime.Today.AddYears(-18))
            .WithMessage("É necessário ter mais de 18 anos.");

        RuleFor(x => x.Weight).NotEmpty()
            .WithMessage("Peso é obrigatório.")
            .GreaterThan(50)
            .WithMessage("Você deve pesar acima de 50kg.");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Gênero inválido.")
            .NotEmpty()
            .WithMessage("Gênero é obrigatório.");
    }
}
