using Inventory.Application.Features.Customers.Dtos;
using Inventory.Domain.Customers;

namespace Inventory.Application.Features.Customers.Mappers;

public static class CustomerMapper
{
    public static CustomerDto ToDto(this Customer customer) => new(customer.Id, customer.Name, customer.Email);
}
