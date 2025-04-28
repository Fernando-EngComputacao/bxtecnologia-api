using AutoMapper;
using BXTecnologia.API.Models.Customer;
using BXTecnologia.API.Models.Customer.DTO;
using BXTecnologia.API.Repositories.Interfaces;
using BXTecnologia.API.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace BXTecnologia.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }
    
    public async Task<bool> CreateAsync(CreateCustomerDTO customerDTO)
    {
        var customer = _mapper.Map<Customer>(customerDTO);
        customer.UpdatedAt = DateTime.UtcNow;
        Console.WriteLine(customer);
        var existingUser = await _customerRepository.GetAsync(customer.Id);
        if (existingUser is not null)
        {
            var message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message);
        }
        
        return await _customerRepository.CreateAsync(customerDTO);
    }

    public async Task<ReadCustomerDTO?> GetAsync(Guid id)
    {
        var customerDto = await _customerRepository.GetAsync(id);
        return _mapper.Map<ReadCustomerDTO>(customerDto);
    }

    public async Task<IEnumerable<ReadCustomerDTO?>> GetAllAsync()
    {
        var customerDtos = await _customerRepository.GetAllAsync();
        return customerDtos.Select(x => x);
    }

    public async Task<bool> UpdateAsync(UpdateCustomerDTO customerDTO, DateTime requestStarted)
    {
        return await _customerRepository.UpdateAsync(customerDTO, requestStarted);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _customerRepository.DeleteAsync(id);
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new []
        {
            new ValidationFailure(paramName, message)
        };
    }
}