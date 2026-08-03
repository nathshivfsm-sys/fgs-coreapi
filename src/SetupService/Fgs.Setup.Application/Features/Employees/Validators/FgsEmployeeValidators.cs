using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.PatchFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.UpdateFgsEmployee;
using Fgs.Setup.Domain.Entities;
using FluentValidation;

namespace Fgs.Setup.Application.Features.Employees.Validators;

public sealed class CreateFgsEmployeeCommandValidator : AbstractValidator<CreateFgsEmployeeCommand>
{
    public CreateFgsEmployeeCommandValidator(IFgsEmployeeReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Request body is required. Ensure the JSON is valid and Content-Type is application/json.");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.EmployeeNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.EmployeeNumber).MustAsync(async (_, employeeNumber, cancellationToken) =>
                    string.IsNullOrWhiteSpace(employeeNumber)
                    || !await readRepository.ExistsByEmployeeNumberAsync(employeeNumber, null, cancellationToken))
                .WithMessage("An employee with this employee number already exists.");

            RuleFor(x => x.Dto.EmployeeTypeId)
                .Must(id => id is EmployeeTypeIds.Office or EmployeeTypeIds.Technician)
                .WithMessage("EmployeeTypeId must be Office (1) or Technician (2).");

            RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.LegalFirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.LegalMiddleName).MaximumLength(100).When(x => x.Dto.LegalMiddleName is not null);
            RuleFor(x => x.Dto.LegalLastName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Dto.StatusId)
                .Must(id => id is EmployeeStatusIds.Active
                    or EmployeeStatusIds.Inactive
                    or EmployeeStatusIds.LeaveOfAbsence
                    or EmployeeStatusIds.Terminated)
                .WithMessage("StatusId must be Active (1), Inactive (2), LeaveOfAbsence (3), or Terminated (4).");

            RuleFor(x => x.Dto.PersonalEmail).MaximumLength(255).When(x => x.Dto.PersonalEmail is not null);
            RuleFor(x => x.Dto.OfficeEmail).MaximumLength(255).When(x => x.Dto.OfficeEmail is not null);
            RuleFor(x => x.Dto.PersonalPhone).MaximumLength(25).When(x => x.Dto.PersonalPhone is not null);
            RuleFor(x => x.Dto.OfficePhone).MaximumLength(25).When(x => x.Dto.OfficePhone is not null);

            RuleFor(x => x.Dto.LaborBurdenTypeId)
                .Must(id => id is LaborBurdenTypeIds.Percentage or LaborBurdenTypeIds.Amount)
                .When(x => x.Dto.LaborBurdenTypeId.HasValue)
                .WithMessage("LaborBurdenTypeId must be Percentage (1) or Amount (2).");

            RuleFor(x => x.Dto.RegularRate).GreaterThanOrEqualTo(0).When(x => x.Dto.RegularRate.HasValue);
            RuleFor(x => x.Dto.OvertimeRate).GreaterThanOrEqualTo(0).When(x => x.Dto.OvertimeRate.HasValue);
            RuleFor(x => x.Dto.DoubleTimeRate).GreaterThanOrEqualTo(0).When(x => x.Dto.DoubleTimeRate.HasValue);
            RuleFor(x => x.Dto.LaborBurdenValue).GreaterThanOrEqualTo(0).When(x => x.Dto.LaborBurdenValue.HasValue);

            RuleFor(x => x.Dto.UserId)
                .MustAsync(async (command, userId, cancellationToken) =>
                    userId is null || !await readRepository.ExistsByUserIdAsync(userId.Value, null, cancellationToken))
                .WithMessage("An employee linked to this user already exists.");

            RuleFor(x => x.Dto.Address).NotNull();
            RuleFor(x => x.Dto.Address)
                .SetValidator(new EmployeeAddressWriteDtoValidator()!)
                .When(x => x.Dto.Address is not null);
        });
    }
}

public sealed class UpdateFgsEmployeeCommandValidator : AbstractValidator<UpdateFgsEmployeeCommand>
{
    public UpdateFgsEmployeeCommandValidator(IFgsEmployeeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Request body is required. Ensure the JSON is valid and Content-Type is application/json.");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.EmployeeNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.EmployeeNumber).MustAsync(async (command, employeeNumber, cancellationToken) =>
                    string.IsNullOrWhiteSpace(employeeNumber)
                    || !await readRepository.ExistsByEmployeeNumberAsync(employeeNumber, command.Id, cancellationToken))
                .WithMessage("An employee with this employee number already exists.");

            RuleFor(x => x.Dto.EmployeeTypeId)
                .Must(id => id is EmployeeTypeIds.Office or EmployeeTypeIds.Technician)
                .WithMessage("EmployeeTypeId must be Office (1) or Technician (2).");

            RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.LegalFirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.LegalMiddleName).MaximumLength(100).When(x => x.Dto.LegalMiddleName is not null);
            RuleFor(x => x.Dto.LegalLastName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Dto.StatusId)
                .Must(id => id is EmployeeStatusIds.Active
                    or EmployeeStatusIds.Inactive
                    or EmployeeStatusIds.LeaveOfAbsence
                    or EmployeeStatusIds.Terminated)
                .WithMessage("StatusId must be Active (1), Inactive (2), LeaveOfAbsence (3), or Terminated (4).");

            RuleFor(x => x.Dto.PersonalEmail).MaximumLength(255).When(x => x.Dto.PersonalEmail is not null);
            RuleFor(x => x.Dto.OfficeEmail).MaximumLength(255).When(x => x.Dto.OfficeEmail is not null);
            RuleFor(x => x.Dto.PersonalPhone).MaximumLength(25).When(x => x.Dto.PersonalPhone is not null);
            RuleFor(x => x.Dto.OfficePhone).MaximumLength(25).When(x => x.Dto.OfficePhone is not null);

            RuleFor(x => x.Dto.LaborBurdenTypeId)
                .Must(id => id is LaborBurdenTypeIds.Percentage or LaborBurdenTypeIds.Amount)
                .When(x => x.Dto.LaborBurdenTypeId.HasValue)
                .WithMessage("LaborBurdenTypeId must be Percentage (1) or Amount (2).");

            RuleFor(x => x.Dto.RegularRate).GreaterThanOrEqualTo(0).When(x => x.Dto.RegularRate.HasValue);
            RuleFor(x => x.Dto.OvertimeRate).GreaterThanOrEqualTo(0).When(x => x.Dto.OvertimeRate.HasValue);
            RuleFor(x => x.Dto.DoubleTimeRate).GreaterThanOrEqualTo(0).When(x => x.Dto.DoubleTimeRate.HasValue);
            RuleFor(x => x.Dto.LaborBurdenValue).GreaterThanOrEqualTo(0).When(x => x.Dto.LaborBurdenValue.HasValue);

            RuleFor(x => x.Dto.UserId)
                .MustAsync(async (command, userId, cancellationToken) =>
                    userId is null || !await readRepository.ExistsByUserIdAsync(userId.Value, command.Id, cancellationToken))
                .WithMessage("An employee linked to this user already exists.");

            RuleFor(x => x.Dto.Address).NotNull();
            RuleFor(x => x.Dto.Address)
                .SetValidator(new EmployeeAddressWriteDtoValidator()!)
                .When(x => x.Dto.Address is not null);
        });
    }
}

public sealed class PatchFgsEmployeeCommandValidator : AbstractValidator<PatchFgsEmployeeCommand>
{
    public PatchFgsEmployeeCommandValidator(IFgsEmployeeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Request body is required. Ensure the JSON is valid and Content-Type is application/json.");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.EmployeeNumber).NotEmpty().MaximumLength(50).When(x => x.Dto.EmployeeNumber is not null);
            RuleFor(x => x.Dto.EmployeeNumber).MustAsync(async (command, employeeNumber, cancellationToken) =>
                    employeeNumber is null
                    || string.IsNullOrWhiteSpace(employeeNumber)
                    || !await readRepository.ExistsByEmployeeNumberAsync(employeeNumber, command.Id, cancellationToken))
                .When(x => x.Dto.EmployeeNumber is not null)
                .WithMessage("An employee with this employee number already exists.");

            RuleFor(x => x.Dto.EmployeeTypeId)
                .Must(id => id is EmployeeTypeIds.Office or EmployeeTypeIds.Technician)
                .When(x => x.Dto.EmployeeTypeId.HasValue)
                .WithMessage("EmployeeTypeId must be Office (1) or Technician (2).");

            RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200).When(x => x.Dto.DisplayName is not null);
            RuleFor(x => x.Dto.LegalFirstName).NotEmpty().MaximumLength(100).When(x => x.Dto.LegalFirstName is not null);
            RuleFor(x => x.Dto.LegalMiddleName).MaximumLength(100).When(x => x.Dto.LegalMiddleName is not null);
            RuleFor(x => x.Dto.LegalLastName).NotEmpty().MaximumLength(100).When(x => x.Dto.LegalLastName is not null);

            RuleFor(x => x.Dto.StatusId)
                .Must(id => id is EmployeeStatusIds.Active
                    or EmployeeStatusIds.Inactive
                    or EmployeeStatusIds.LeaveOfAbsence
                    or EmployeeStatusIds.Terminated)
                .When(x => x.Dto.StatusId.HasValue)
                .WithMessage("StatusId must be Active (1), Inactive (2), LeaveOfAbsence (3), or Terminated (4).");

            RuleFor(x => x.Dto.PersonalEmail).MaximumLength(255).When(x => x.Dto.PersonalEmail is not null);
            RuleFor(x => x.Dto.OfficeEmail).MaximumLength(255).When(x => x.Dto.OfficeEmail is not null);
            RuleFor(x => x.Dto.PersonalPhone).MaximumLength(25).When(x => x.Dto.PersonalPhone is not null);
            RuleFor(x => x.Dto.OfficePhone).MaximumLength(25).When(x => x.Dto.OfficePhone is not null);

            RuleFor(x => x.Dto.LaborBurdenTypeId)
                .Must(id => id is LaborBurdenTypeIds.Percentage or LaborBurdenTypeIds.Amount)
                .When(x => x.Dto.LaborBurdenTypeId.HasValue)
                .WithMessage("LaborBurdenTypeId must be Percentage (1) or Amount (2).");

            RuleFor(x => x.Dto.RegularRate).GreaterThanOrEqualTo(0).When(x => x.Dto.RegularRate.HasValue);
            RuleFor(x => x.Dto.OvertimeRate).GreaterThanOrEqualTo(0).When(x => x.Dto.OvertimeRate.HasValue);
            RuleFor(x => x.Dto.DoubleTimeRate).GreaterThanOrEqualTo(0).When(x => x.Dto.DoubleTimeRate.HasValue);
            RuleFor(x => x.Dto.LaborBurdenValue).GreaterThanOrEqualTo(0).When(x => x.Dto.LaborBurdenValue.HasValue);

            RuleFor(x => x.Dto.UserId)
                .MustAsync(async (command, userId, cancellationToken) =>
                    userId is null || !await readRepository.ExistsByUserIdAsync(userId.Value, command.Id, cancellationToken))
                .When(x => x.Dto.UserId.HasValue)
                .WithMessage("An employee linked to this user already exists.");

            RuleFor(x => x.Dto.Address)
                .SetValidator(new EmployeeAddressWriteDtoValidator()!)
                .When(x => x.Dto.Address is not null);
        });
    }
}

internal sealed class EmployeeAddressWriteDtoValidator : AbstractValidator<LocationWriteDto>
{
    public EmployeeAddressWriteDtoValidator()
    {
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AddressLine2).MaximumLength(200).When(x => x.AddressLine2 is not null);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.County).MaximumLength(100).When(x => x.County is not null);
        RuleFor(x => x.FormattedAddress).MaximumLength(1000).When(x => x.FormattedAddress is not null);
        RuleFor(x => x.PlaceId).MaximumLength(500).When(x => x.PlaceId is not null);
    }
}
