using FluentValidation;
namespace Inventory.Application.Features.Warehouses.Commands.UpdateWarehouse;
public sealed class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
{
    public UpdateWarehouseCommandValidator() { RuleFor(x => x.Id).NotEmpty(); RuleFor(x => x.Name).NotEmpty().MaximumLength(200); }
}
