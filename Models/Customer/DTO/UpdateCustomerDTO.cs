namespace BXTecnologia.API.Models.Customer.DTO;

public class UpdateCustomerDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Level { get; set; }
    public string Role { get; set; }
}