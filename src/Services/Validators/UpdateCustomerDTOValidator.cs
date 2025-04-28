using BXTecnologia.API.Models.Customer.DTO;
using FluentValidation;

namespace BXTecnologia.API.Services.Validators;

public class UpdateCustomerDTOValidator : AbstractValidator<UpdateCustomerDTO>
{
    public UpdateCustomerDTOValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("O nome completo é obrigatório")
            .MinimumLength(3).WithMessage("O nome completo deve ter pelo menos 3 caracteres")
            .MaximumLength(100).WithMessage("O nome completo não pode exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("O email fornecido não é válido");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória")
            .LessThan(DateTime.Now).WithMessage("A data de nascimento não pode ser no futuro");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("O nível é obrigatório")
            .Must(level => new[] { "Beginner", "Intermediate", "Advanced" }.Contains(level))
            .WithMessage("O nível deve ser 'Beginner', 'Intermediate' ou 'Advanced'");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("O cargo é obrigatório")
            .MaximumLength(50).WithMessage("O cargo não pode exceder 50 caracteres");
    }
}
