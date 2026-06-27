using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Commands.CreateFgsWarehouse;
using Fgs.Setup.Application.Features.Warehouses.Commands.PatchFgsWarehouse;
using Fgs.Setup.Application.Features.Warehouses.Commands.UpdateFgsWarehouse;
using FluentValidation;

namespace Fgs.Setup.Application.Features.Warehouses.Validators;

public sealed class CreateFgsWarehouseCommandValidator : AbstractValidator<CreateFgsWarehouseCommand>
{
    public CreateFgsWarehouseCommandValidator(IFgsWarehouseReadRepository readRepository)
    {
        RuleFor(x => x.Dto.WarehouseCode).NotEmpty();
        RuleFor(x => x.Dto.WarehouseCode).MaximumLength(50);
        RuleFor(x => x.Dto.WarehouseCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("WarehouseCode must be uppercase.");
        RuleFor(x => x.Dto.WarehouseCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByWarehouseCodeAsync(code, null, cancellationToken))
            .WithMessage("A warehouse with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.WarehouseType).NotEmpty();
        RuleFor(x => x.Dto.WarehouseType).MaximumLength(30);


        RuleFor(x => x.Dto.WarehouseCode).NotEmpty();
        RuleFor(x => x.Dto.WarehouseCode).MaximumLength(50);
        RuleFor(x => x.Dto.WarehouseCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("WarehouseCode must be uppercase.");
        RuleFor(x => x.Dto.WarehouseCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByWarehouseCodeAsync(code, null, cancellationToken))
            .WithMessage("A warehouse with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.WarehouseType).NotEmpty();
        RuleFor(x => x.Dto.WarehouseType).MaximumLength(30);



    }
}

public sealed class UpdateFgsWarehouseCommandValidator : AbstractValidator<UpdateFgsWarehouseCommand>
{
    public UpdateFgsWarehouseCommandValidator(IFgsWarehouseReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.WarehouseCode).NotEmpty();
        RuleFor(x => x.Dto.WarehouseCode).MaximumLength(50);
        RuleFor(x => x.Dto.WarehouseCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("WarehouseCode must be uppercase.");
        RuleFor(x => x.Dto.WarehouseCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByWarehouseCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A warehouse with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.WarehouseType).NotEmpty();
        RuleFor(x => x.Dto.WarehouseType).MaximumLength(30);



    }
}

public sealed class PatchFgsWarehouseCommandValidator : AbstractValidator<PatchFgsWarehouseCommand>
{
    public PatchFgsWarehouseCommandValidator(IFgsWarehouseReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.WarehouseCode).NotEmpty();
        RuleFor(x => x.Dto.WarehouseCode).MaximumLength(50);
        RuleFor(x => x.Dto.WarehouseCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("WarehouseCode must be uppercase.");
        RuleFor(x => x.Dto.WarehouseCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByWarehouseCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A warehouse with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.WarehouseType).NotEmpty();
        RuleFor(x => x.Dto.WarehouseType).MaximumLength(30);



    }
}
