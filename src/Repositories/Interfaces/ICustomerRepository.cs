using BXTecnologia.API.Models.Customer;
using BXTecnologia.API.Models.Customer.DTO;

namespace BXTecnologia.API.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(CreateCustomerDTO customer);

    Task<ReadCustomerDTO?> GetAsync(Guid id);

    Task<IEnumerable<ReadCustomerDTO>> GetAllAsync();

    Task<bool> UpdateAsync(UpdateCustomerDTO customer, DateTime requestStarted);

    Task<bool> DeleteAsync(Guid id);
}