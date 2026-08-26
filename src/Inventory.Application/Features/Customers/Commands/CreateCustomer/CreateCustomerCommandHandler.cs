using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Customers.Dtos;
using Inventory.Application.Features.Customers.Mappers;
using Inventory.Domain.Common.Results;
using Inventory.Domain.Customers;
using MediatR;

namespace Inventory.Application.Features.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler(IAppDbContext context)
    : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        var result = Customer.Create(Guid.NewGuid(), command.Name, command.Email);
        if (result.IsError) return result.Errors;

        context.Customers.Add(result.Value);
        await context.SaveChangesAsync(ct);
        return result.Value.ToDto();
    }
}
