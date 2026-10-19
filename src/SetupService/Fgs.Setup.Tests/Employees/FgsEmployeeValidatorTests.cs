using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.PatchFgsEmployee;
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
    public async Task CreateValidator_WhenEmployeeNumberMissing_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);

        var missing = await validator.ValidateAsync(new CreateFgsEmployeeCommand(CreateDto() with { EmployeeNumber = null }));
        var blank = await validator.ValidateAsync(new CreateFgsEmployeeCommand(CreateDto(employeeNumber: "")));

        missing.IsValid.Should().BeTrue();
        blank.IsValid.Should().BeTrue();
        missing.Errors.Should().NotContain(e => e.PropertyName == "Dto.EmployeeNumber");
        blank.Errors.Should().NotContain(e => e.PropertyName == "Dto.EmployeeNumber");
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
    public async Task CreateValidator_WhenDuplicateOrSpecialEmployeeNumber_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);

        var duplicate = await validator.ValidateAsync(new CreateFgsEmployeeCommand(CreateDto()));
        var specialCharacters = await validator.ValidateAsync(
            new CreateFgsEmployeeCommand(CreateDto(employeeNumber: "EMP@001")));

        duplicate.IsValid.Should().BeTrue();
        specialCharacters.IsValid.Should().BeTrue();
        readRepository.Verify(
            r => r.ExistsByEmployeeNumberAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateValidator_WhenCodeStillHasSpecialCharactersAfterTrim_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);

        var employeeNumberResult = await validator.ValidateAsync(
            new CreateFgsEmployeeCommand(CreateDto(employeeNumber: "  EMP@001  ")));
        var techCodeResult = await validator.ValidateAsync(
            new CreateFgsEmployeeCommand(CreateDto() with
            {
                TechnicianProfile = CreateTechnicianProfile() with { TechCode = "  T@001  " }
            }));

        employeeNumberResult.IsValid.Should().BeTrue();
        techCodeResult.IsValid.Should().BeFalse();
        techCodeResult.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("special characters", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenCodeHasOnlyLeadingOrTrailingSpaces_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto(employeeNumber: "  EMP-001  ") with
        {
            TechnicianProfile = CreateTechnicianProfile() with { TechCode = "  T-001  " }
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenLegalNamesNull_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto() with
        {
            LegalFirstName = null,
            LegalMiddleName = null,
            LegalLastName = null
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenBlankTechCodeUsesEmployeeNumber_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto(employeeNumber: "  EMP-100  ") with
        {
            TechnicianProfile = CreateTechnicianProfile() with { TechCode = "   " }
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenBlankTechCode_PassesWithoutCheckingEmployeeNumber()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto(employeeNumber: "  " + new string('A', 26) + "  ") with
        {
            TechnicianProfile = CreateTechnicianProfile() with { TechCode = null }
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WhenEmployeeNumberHasOnlyLeadingOrTrailingSpaces_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new UpdateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new UpdateFgsEmployeeCommand(
            5,
            new FgsEmployeeUpdateDto(
                null,
                "  EMP-001  ",
                EmployeeTypeIds.Office,
                "Alex Office",
                null,
                "   ",
                null,
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

    [Fact]
    public async Task UpdateValidator_WhenEmployeeNumberMissing_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new UpdateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new UpdateFgsEmployeeCommand(5, CreateOfficeUpdateDto(employeeNumber: ""));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.EmployeeNumber");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateEmployeeNumber_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync("EMP-001", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new UpdateFgsEmployeeCommand(5, CreateOfficeUpdateDto());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("employee number", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateValidator_WhenEmployeeNumberHasSpecialCharacters_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new UpdateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new UpdateFgsEmployeeCommand(5, CreateOfficeUpdateDto(employeeNumber: "EMP@001"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("special characters", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateValidator_WhenTechCodeHasOnlyLeadingOrTrailingSpaces_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new UpdateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new UpdateFgsEmployeeCommand(
            5,
            new FgsEmployeeUpdateDto(
                null,
                "  EMP-001  ",
                EmployeeTypeIds.Technician,
                "Alex Tech",
                null,
                null,
                null,
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
                null,
                null,
                false,
                null,
                CreateTechnicianProfile() with { TechCode = "  T-001  " }));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WhenTechCodeBlank_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new UpdateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new UpdateFgsEmployeeCommand(
            5,
            new FgsEmployeeUpdateDto(
                null,
                "EMP-001",
                EmployeeTypeIds.Technician,
                "Alex Tech",
                "Alex",
                null,
                "Tech",
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
                null,
                null,
                false,
                null,
                CreateTechnicianProfile() with { TechCode = "   " }));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TechnicianProfile.TechCode");
    }

    [Fact]
    public async Task PatchValidator_WhenCodesHaveOnlyLeadingOrTrailingSpaces_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new PatchFgsEmployeeCommandValidator(readRepository.Object);
        var command = new PatchFgsEmployeeCommand(
            5,
            new FgsEmployeePatchDto(
                null,
                "  EMP-001  ",
                EmployeeTypeIds.Technician,
                null,
                "   ",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                CreateTechnicianProfile() with { TechCode = "  T-001  " }));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_WhenEmployeeNumberStillHasSpecialCharactersAfterTrim_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new PatchFgsEmployeeCommandValidator(readRepository.Object);
        var command = new PatchFgsEmployeeCommand(
            5,
            new FgsEmployeePatchDto(
                null,
                "  EMP@001  ",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("special characters", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task PatchValidator_WhenEmployeeNumberMissing_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new PatchFgsEmployeeCommandValidator(readRepository.Object);
        var command = new PatchFgsEmployeeCommand(5, CreateEmployeeNumberPatch(""));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.EmployeeNumber");
    }

    [Fact]
    public async Task PatchValidator_WhenDuplicateEmployeeNumber_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync("EMP-001", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new PatchFgsEmployeeCommandValidator(readRepository.Object);
        var command = new PatchFgsEmployeeCommand(5, CreateEmployeeNumberPatch("EMP-001"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("employee number", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenTechnicianWithoutProfile_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto(includeTechnicianProfile: false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("Technician profile is required", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateTechCode_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByTechCodeAsync("T-001", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("tech code", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateOfficeEmail_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByOfficeEmailAsync("alex@example.com", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("office email", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenInvalidOfficeEmailFormat_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto() with { OfficeEmail = "user@domain,com" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Dto.OfficeEmail"
            && e.ErrorMessage.Contains("email", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenInvalidPersonalEmailFormat_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var validator = new CreateFgsEmployeeCommandValidator(readRepository.Object);
        var command = new CreateFgsEmployeeCommand(CreateDto() with { PersonalEmail = "not-an-email" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Dto.PersonalEmail"
            && e.ErrorMessage.Contains("email", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateValidator_WhenOfficeEmailBelongsToSelf_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        readRepository
            .Setup(r => r.ExistsByOfficeEmailAsync("alex@example.com", 5, It.IsAny<CancellationToken>()))
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
        readRepository.Verify(
            r => r.ExistsByOfficeEmailAsync("alex@example.com", 5, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateOfficeEmail_HasValidationError()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        readRepository
            .Setup(r => r.ExistsByOfficeEmailAsync("taken@example.com", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

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
                "taken@example.com",
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

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("office email", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateValidator_WhenValidDto_Passes()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ExistsByEmployeeNumberAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        readRepository
            .Setup(r => r.ExistsByOfficeEmailAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
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

    private static FgsEmployeeUpdateDto CreateOfficeUpdateDto(string employeeNumber = "EMP-001") =>
        new(
            null,
            employeeNumber,
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
            null);

    private static FgsEmployeePatchDto CreateEmployeeNumberPatch(string? employeeNumber) =>
        new(
            null,
            employeeNumber,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

    private static FgsEmployeeCreateDto CreateDto(
        string employeeNumber = "EMP-001",
        string addressLine1 = "100 Main St",
        bool includeTechnicianProfile = true) =>
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
            Notes: null,
            TechnicianProfile: includeTechnicianProfile ? CreateTechnicianProfile() : null);

    private static FgsEmployeeTechnicianProfileWriteDto CreateTechnicianProfile() =>
        new(
            TechCode: "T-001",
            TechName: "Alex",
            CanBeScheduled: true,
            DailyCapacityHours: 8m,
            DispatchZoneId: null,
            StartLocationTypeId: StartLocationTypeIds.Office,
            StartTime: new TimeOnly(8, 0),
            TechTradeId: null,
            TechSkillId: null,
            TruckId: null,
            CustomerFacingPhone: null,
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
