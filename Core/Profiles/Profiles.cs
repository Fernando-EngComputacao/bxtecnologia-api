using AutoMapper;
using BXTecnologia.API.Models.Customer;
using BXTecnologia.API.Models.Customer.DTO;

namespace BXTecnologia.API.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        #region Customer

        CreateMap<Customer, ReadCustomerDTO>();
        CreateMap<ReadCustomerDTO, Customer>();
        CreateMap<CreateCustomerDTO, Customer>();
        CreateMap<UpdateCustomerDTO, Customer>();
        
        #endregion
    }
}