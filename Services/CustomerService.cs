using AutoMapper;
using BXTecnologia.API.Config.Interfaces;
using BXTecnologia.API.Models.Customer;
using BXTecnologia.API.Models.Customer.DTO;
using BXTecnologia.API.Repositories.Interfaces;
using BXTecnologia.API.Services.Interfaces;
using BXTecnologia.API.Services.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace BXTecnologia.API.Services;

public class CustomerService : ICustomerService
{
    private readonly IMapper _mapper;
    private readonly IEmailLayout _emailLayout;
    private readonly IEmailService _emailService;
    private readonly ICustomerRepository _customerRepository;
    private readonly CreateCustomerDTOValidator _createValidator;
    private readonly UpdateCustomerDTOValidator _updateValidator;

    public CustomerService(
        ICustomerRepository customerRepository,
        IMapper mapper, IEmailService emailService, IEmailLayout emailLayout)
    {
        _mapper = mapper;
        _emailLayout = emailLayout;
        _emailService = emailService;
        _customerRepository = customerRepository;
        _createValidator = new CreateCustomerDTOValidator();
        _updateValidator = new UpdateCustomerDTOValidator();
    }

    public async Task<bool> CreateAsync(CreateCustomerDTO customerDTO)
    {
        var validationResult = await _createValidator.ValidateAsync(customerDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var customer = _mapper.Map<Customer>(customerDTO);
        customer.UpdatedAt = DateTime.UtcNow;

        var existingUser = await _customerRepository.GetAsync(customer.Id);
        if (existingUser is not null)
        {
            var message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message);
        }

        var response = await _customerRepository.CreateAsync(customerDTO);
        var mensageLevel = customer.Level.StartsWith("Beginner")
            ? "usuário nível Básico, com limite diário de upload de 5 imagens"
            : "usário nível Prêmium, com upload ilimitado de imagens";

        if (response)
        {
            _emailService.SendEmailAsync(customer.Email,
                "Cadastro Realizado com Sucesso",
                _emailLayout.GetUserRegistrationEmailTemplate(
                    customer.FullName,
                    mensageLevel
                )
            );
        }

        return response;
    }

    public async Task<ReadCustomerDTO?> GetAsync(Guid id)
    {
        var customerDto = await _customerRepository.GetAsync(id);
        return _mapper.Map<ReadCustomerDTO>(customerDto);
    }

    public async Task<IEnumerable<ReadCustomerDTO?>> GetAllAsync()
    {
        var customerDtos = await _customerRepository.GetAllAsync();
        return customerDtos.Select(x => x).OrderBy(x => x.FullName);
    }

    public async Task<bool> UpdateAsync(UpdateCustomerDTO customerDTO, DateTime requestStarted)
    {
        var validationResult = await _updateValidator.ValidateAsync(customerDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await _customerRepository.UpdateAsync(customerDTO, requestStarted);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _customerRepository.DeleteAsync(id);
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}