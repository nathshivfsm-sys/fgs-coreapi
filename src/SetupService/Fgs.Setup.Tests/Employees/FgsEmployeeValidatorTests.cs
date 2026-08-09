using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.UpdateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Application.Features.Employees.Validators;
using Fgs.Setup.Domain.Entities;
using Moq;

namespace Fgs.Setup.Tests.Employees;

public sealed class FgsEmployeeValidatorTests
{
    [Fact]
    public async Task CreateValidator_WhenDtoNull_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(null!);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto");
        result.Errors.Should().NotContain(e =>
            e.ErrorMessage.Contains("NullReferenceException", StringComparison.Ordinal));
    }

    [Fact]
    public async Task CreateValidator_WhenEmployeeNumberMissing_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto(employeeNumber: ""));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.EmployeeNumber");
    }

    [Fact]
    public async Task CreateValidator_WhenAddressLine1Missing_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto(addressLine1: ""));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Address.AddressLine1");
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateEmployeeNumber_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync("EMP-001", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("employee number", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenEmployeeNumberHasSpecialCharacters_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto(employeeNumber: "EMP@001"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("special characters", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateValidator_WhenValidDto_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new UpdateFgsEmployeeCommand(
            5,
            new FgsEmployeeUpdateDto(
                null,
                "EMP-001",
                EmployeeTypeIds.Office,
                "Alex Office",
                "Alex",
                null,
                "Office",
                null,
                new DateOnly(2026, 1, 15),
                null,
                EmployeeStatusIds.Active,
                null,
                "alex@example.com",
                null,
                "+15551234567",
                CreateAddress(),
                null,
                40m,
                60m,
                80m,
                LaborBurdenTypeIds.Amount,
                10m,
                false,
                null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    private static FgsEmployeeCreateDto CreateDto(
        string employeeNumber = "EMP-001",
        string addressLine1 = "100 Main St") =>
        new(
            UserId: null,
            EmployeeNumber: employeeNumber,
            EmployeeTypeId: EmployeeTypeIds.Technician,
            DisplayName: "Alex Tech",
            LegalFirstName: "Alex",
            LegalMiddleName: null,
            LegalLastName: "Tech",
            BirthDate: null,
            HireDate: new DateOnly(2026, 1, 15),
            TerminationDate: null,
            StatusId: EmployeeStatusIds.Active,
            PersonalEmail: null,
            OfficeEmail: "alex@example.com",
            PersonalPhone: null,
            OfficePhone: "+15551234567",
            Address: CreateAddress(addressLine1),
            ProfilePhotoFileId: null,
            RegularRate: 40m,
            LaborBurdenTypeId: LaborBurdenTypeIds.Percentage,
            LaborBurdenValue: 25m,
            IsPurchaser: false,
            Notes: null);

    private static LocationWriteDto CreateAddress(string addressLine1 = "100 Main St") =>
        new(
            addressLine1,
            "Apt 2",
            null,
            null,
            "Austin",
            "TX",
            null,
            "US",
            "78701",
            null,
            null,
            null,
            null);
}
