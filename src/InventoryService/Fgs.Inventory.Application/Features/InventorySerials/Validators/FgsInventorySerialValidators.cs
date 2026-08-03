using Fgs.Inventory.Application.Abstractions.InventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Commands.CreateFgsInventorySerial;
using Fgs.Inventory.Application.Features.InventorySerials.Commands.PatchFgsInventorySerial;
using Fgs.Inventory.Application.Features.InventorySerials.Commands.UpdateFgsInventorySerial;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventorySerials.Validators;

public sealed class CreateFgsInventorySerialCommandValidator : AbstractValidator<CreateFgsInventorySerialCommand>
{
    public CreateFgsInventorySerialCommandValidator(IFgsInventorySerialReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item was not found or is inactive.")
                .When(x => x.Dto.InventoryItemId > 0);
            RuleFor(x => x.Dto.SerialNumber).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.SerialNumber)
                .MustAsync(async (command, serialNumber, cancellationToken) =>
                    !await readRepository.ExistsBySerialNumberAsync(
                        command.Dto.InventoryItemId,
                        serialNumber,
                        null,
                        cancellationToken))
                .WithMessage("A serial number with this value already exists for the inventory item.")
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.SerialNumber) && x.Dto.InventoryItemId > 0);
            RuleFor(x => x.Dto.InventorySerialStatus).IsInEnum();
        });
    }
}

public sealed class UpdateFgsInventorySerialCommandValidator : AbstractValidator<UpdateFgsInventorySerialCommand>
{
    public UpdateFgsInventorySerialCommandValidator(IFgsInventorySerialReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryItemId).GreaterThan(0);
            RuleFor(x => x.Dto.InventoryItemId)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item was not found or is inactive.")
                .When(x => x.Dto.InventoryItemId > 0);
            RuleFor(x => x.Dto.SerialNumber).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.SerialNumber)
                .MustAsync(async (command, serialNumber, cancellationToken) =>
                    !await readRepository.ExistsBySerialNumberAsync(
                        command.Dto.InventoryItemId,
                        serialNumber,
                        command.Id,
                        cancellationToken))
                .WithMessage("A serial number with this value already exists for the inventory item.")
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.SerialNumber) && x.Dto.InventoryItemId > 0);
            RuleFor(x => x.Dto.InventorySerialStatus).IsInEnum();
        });
    }
}

public sealed class PatchFgsInventorySerialCommandValidator : AbstractValidator<PatchFgsInventorySerialCommand>
{
    public PatchFgsInventorySerialCommandValidator(IFgsInventorySerialReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InventoryItemId!.Value)
                .GreaterThan(0)
                .MustAsync(async (itemId, cancellationToken) =>
                    await readRepository.ExistsInventoryItemAsync(itemId, cancellationToken))
                .WithMessage("Inventory item was not found or is inactive.")
                .When(x => x.Dto.InventoryItemId.HasValue);
            RuleFor(x => x.Dto.SerialNumber).NotEmpty().MaximumLength(200)
                .When(x => x.Dto.SerialNumber is not null);
            RuleFor(x => x.Dto.SerialNumber)
                .MustAsync(async (command, serialNumber, cancellationToken) =>
                    !await readRepository.ExistsBySerialNumberAsync(
                        command.Dto.InventoryItemId ?? 0,
                        serialNumber!,
                        command.Id,
                        cancellationToken))
                .WithMessage("A serial number with this value already exists for the inventory item.")
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.SerialNumber));
            RuleFor(x => x.Dto.InventorySerialStatus!.Value).IsInEnum()
                .When(x => x.Dto.InventorySerialStatus.HasValue);
        });
    }
}
