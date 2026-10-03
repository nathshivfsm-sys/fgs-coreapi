using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerServiceLocation;
using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Features.Customers.Validators;
using Fgs.Crm.Domain.Enums;
using Moq;

namespace Fgs.Crm.Tests.Customers;

public sealed class CrmCustomerValidatorTests
{
    private readonly Mock<ICrmCustomerReadRepository> _readRepository = new();

    private static CrmCustomerCreateDto SampleCreateDto(string customerNumber = "CUST01") =>
        new(
            customerNumber,
            "Acme Corporation",
            "Acme Corp",
            "100 Main St",
            null,
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
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            null,
            null,
            null,
            null);

    [Fact]
    public async Task CreateValidator_WhenCustomerNumberMissing_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(SampleCreateDto("") with { CustomerNumber = "" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CustomerNumber");
    }

    [Fact]
    public async Task CreateValidator_WhenCustomerNumberNotUppercase_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(SampleCreateDto("cust01"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CustomerNumber");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateNumberExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByCustomerNumberAsync("CUST01", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateCrmCustomerCommandValidator(_readRepository.Object);
        var updateDto = new CrmCustomerUpdateDto(
            "CUST01",
            "Acme Corporation",
            "Acme Corp",
            "100 Main St",
            null,
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
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            null,
            null,
            null,
            null);

        var result = await validator.ValidateAsync(new UpdateCrmCustomerCommand(5, updateDto));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenPaymentTermMissing_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(SampleCreateDto() with { DefaultPaymentTermId = null });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.DefaultPaymentTermId");
    }

    [Fact]
    public async Task CreateValidator_WhenEmailPhoneOrWebsiteInvalid_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            PrimaryContactName = "Jane Doe",
            PrimaryContactEmail = "not-an-email",
            PrimaryContactPhone = "call-me",
            Website = "www.acme.com"
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.PrimaryContactEmail");
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.PrimaryContactPhone");
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Website");
    }

    [Fact]
    public async Task CreateValidator_WhenEmailOrPhoneWithoutName_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            PrimaryContactEmail = "jane@example.com"
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.PrimaryContactName");
    }

    [Fact]
    public async Task CreateValidator_BillToOnly_RejectsLocationPayload()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            ServiceLocation = new CrmServiceLocationCreateDto(ServiceLocationType.Residential)
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation");
    }

    [Fact]
    public async Task CreateValidator_CombinedOption_RequiresServiceLocationType()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var missing = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation
        });
        var invalidType = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto((ServiceLocationType)0)
        });
        var valid = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = ValidLocation()
        });

        var missingResult = await validator.ValidateAsync(missing);
        var invalidTypeResult = await validator.ValidateAsync(invalidType);
        var validResult = await validator.ValidateAsync(valid);

        missingResult.IsValid.Should().BeFalse();
        invalidTypeResult.IsValid.Should().BeFalse();
        invalidTypeResult.Errors.Should().Contain(e => e.PropertyName.Contains("ServiceLocationType"));
        validResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenLocationNameOrDisplayNameMissing_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(ServiceLocationType.Residential, DisplayName: "Site")
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.Name");
    }

    [Fact]
    public async Task AddServiceLocationValidator_RequiresNameAndDisplayName()
    {
        var validator = new AddCrmCustomerServiceLocationCommandValidator();
        var missingName = new AddCrmCustomerServiceLocationCommand(
            1,
            new CrmServiceLocationCreateDto(ServiceLocationType.Commercial, DisplayName: "Warehouse"));
        var missingDisplayName = new AddCrmCustomerServiceLocationCommand(
            1,
            new CrmServiceLocationCreateDto(ServiceLocationType.Commercial, "Warehouse"));
        var valid = new AddCrmCustomerServiceLocationCommand(1, ValidLocation());

        var missingNameResult = await validator.ValidateAsync(missingName);
        var missingDisplayNameResult = await validator.ValidateAsync(missingDisplayName);
        var validResult = await validator.ValidateAsync(valid);

        missingNameResult.IsValid.Should().BeFalse();
        missingNameResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
        missingDisplayNameResult.IsValid.Should().BeFalse();
        missingDisplayNameResult.Errors.Should().Contain(e => e.PropertyName == "Dto.DisplayName");
        validResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task AddServiceLocationValidator_CustomerTypeIsOptionalAndMustBeDefined()
    {
        var validator = new AddCrmCustomerServiceLocationCommandValidator();
        var omitted = new AddCrmCustomerServiceLocationCommand(1, ValidLocation());
        var defined = new AddCrmCustomerServiceLocationCommand(
            1,
            ValidLocation() with { CustomerType = CustomerType.PropertyManagement });
        var undefined = new AddCrmCustomerServiceLocationCommand(
            1,
            ValidLocation() with { CustomerType = (CustomerType)9 });

        var omittedResult = await validator.ValidateAsync(omitted);
        var definedResult = await validator.ValidateAsync(defined);
        var undefinedResult = await validator.ValidateAsync(undefined);

        omittedResult.IsValid.Should().BeTrue();
        definedResult.IsValid.Should().BeTrue();
        undefinedResult.IsValid.Should().BeFalse();
        undefinedResult.Errors.Should().Contain(e => e.PropertyName == "Dto.CustomerType");
    }

    [Fact]
    public async Task CreateValidator_PartialAddressRequiresLine1PostalStateAndCountry()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var onlyCity = new CreateCrmCustomerCommand(BlankAddress(ValidCreateDto()) with { City = "Austin" });
        var missingLine1 = new CreateCrmCustomerCommand(ValidCreateDto() with { AddressLine1 = null });
        var missingPostal = new CreateCrmCustomerCommand(ValidCreateDto() with { PostalCode = " " });
        var missingState = new CreateCrmCustomerCommand(ValidCreateDto() with { State = null });
        var missingCountry = new CreateCrmCustomerCommand(ValidCreateDto() with { Country = null });

        var onlyCityResult = await validator.ValidateAsync(onlyCity);
        var missingLine1Result = await validator.ValidateAsync(missingLine1);
        var missingPostalResult = await validator.ValidateAsync(missingPostal);
        var missingStateResult = await validator.ValidateAsync(missingState);
        var missingCountryResult = await validator.ValidateAsync(missingCountry);

        onlyCityResult.IsValid.Should().BeFalse();
        onlyCityResult.Errors.Should().Contain(e => e.PropertyName == "Dto.AddressLine1");
        onlyCityResult.Errors.Should().Contain(e => e.PropertyName == "Dto.PostalCode");
        onlyCityResult.Errors.Should().Contain(e => e.PropertyName == "Dto.State");
        onlyCityResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Country");
        onlyCityResult.Errors.Should().NotContain(e => e.PropertyName == "Dto.City");
        missingLine1Result.Errors.Should().Contain(e => e.PropertyName == "Dto.AddressLine1");
        missingPostalResult.Errors.Should().Contain(e => e.PropertyName == "Dto.PostalCode");
        missingStateResult.Errors.Should().Contain(e => e.PropertyName == "Dto.State");
        missingCountryResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Country");
    }

    [Fact]
    public async Task CreateValidator_BlankAddressIsAllowed()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(BlankAddress(ValidCreateDto()) with
        {
            AddressLine2 = "   ",
            City = " "
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_LocationPartialAddressFailsAndBlankAddressIsAllowed()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var partial = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = ValidLocation() with { AddressLine2 = "Suite 2" }
        });
        var blank = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = ValidLocation()
        });

        var partialResult = await validator.ValidateAsync(partial);
        var blankResult = await validator.ValidateAsync(blank);

        partialResult.IsValid.Should().BeFalse();
        partialResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.AddressLine1");
        partialResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.PostalCode");
        partialResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.State");
        partialResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.Country");
        partialResult.Errors.Should().NotContain(e => e.PropertyName == "Dto.ServiceLocation.AddressLine2");
        blankResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicateTagIds()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            TagIds = [4, 4],
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = ValidLocation() with { TagIds = [1, 2, 1] }
        });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TagIds");
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.TagIds");
    }

    private static CrmCustomerCreateDto ValidCreateDto() =>
        SampleCreateDto() with { DefaultPaymentTermId = 5 };

    private static CrmCustomerCreateDto BlankAddress(CrmCustomerCreateDto dto) =>
        dto with
        {
            AddressLine1 = null,
            AddressLine2 = null,
            AddressLine3 = null,
            AddressLine4 = null,
            City = null,
            State = null,
            County = null,
            Country = null,
            PostalCode = null,
            FormattedAddress = null
        };

    private static CrmServiceLocationCreateDto ValidLocation() =>
        new(ServiceLocationType.Residential, "Main", "Main site");
}
