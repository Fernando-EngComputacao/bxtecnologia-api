using BXTecnologia.API.Models.Customer.DTO;

namespace BXTecnologia.API.Services.Interfaces;

public interface ICustomerService
{
    Task<bool> CreateAsync(CreateCustomerDTO customer);

    Task<ReadCustomerDTO?> GetAsync(Guid id);

    Task<IEnumerable<ReadCustomerDTO?>> GetAllAsync();

    Task<bool> UpdateAsync(UpdateCustomerDTO customer, DateTime requestStarted);

    Task<bool> DeleteAsync(Guid id);
}