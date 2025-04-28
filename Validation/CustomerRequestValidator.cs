using BXTecnologia.API.Models.Customer.DTO;
using FluentValidation;

namespace BXTecnologia.API.Validation;

public class CustomerRequestValidator : AbstractValidator<ReadCustomerDTO>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.DateOfBirth).NotEmpty();
    }
}