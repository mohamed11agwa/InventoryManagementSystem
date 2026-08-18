using FluentValidation;
namespace Inventory.Application.Features.Inventory.Commands.AddProductToWarehouse;
public sealed class AddProductToWarehouseCommandValidator : AbstractValidator<AddProductToWarehouseCommand>
{
    public AddProductToWarehouseCommandValidator() { RuleFor(x => x.ProductId).NotEmpty(); RuleFor(x => x.WarehouseId).NotEmpty(); }
}
