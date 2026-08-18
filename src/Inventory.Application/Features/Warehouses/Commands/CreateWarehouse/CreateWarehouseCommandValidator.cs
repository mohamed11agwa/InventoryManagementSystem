using FluentValidation;
namespace Inventory.Application.Features.Warehouses.Commands.CreateWarehouse;
public sealed class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
}
