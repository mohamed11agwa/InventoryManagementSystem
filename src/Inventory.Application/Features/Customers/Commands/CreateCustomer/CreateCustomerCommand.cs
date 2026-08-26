using Inventory.Application.Features.Customers.Dtos;
using Inventory.Domain.Common.Results;
using MediatR;

namespace Inventory.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(string Name, string Email) : IRequest<Result<CustomerDto>>;
