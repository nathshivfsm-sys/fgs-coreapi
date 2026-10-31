using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerNote;
using Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerServiceLocation;
using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.PatchCrmCustomerNote;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomerNote;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomerServiceLocation;
using Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultServiceLocation;
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
    public async Task CreateValidator_WhenPaymentTermMissing_IsValid()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(SampleCreateDto() with { DefaultPaymentTermId = null });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenPaymentTermNotPositive_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);

        foreach (var paymentTermId in new long?[] { 0, -1 })
        {
            var command = new CreateCrmCustomerCommand(SampleCreateDto() with { DefaultPaymentTermId = paymentTermId });
            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Dto.DefaultPaymentTermId");
        }
    }

    [Fact]
    public async Task UpsertDefaultValidators_WhenIdNotPositive_HasValidationError()
    {
        var customerValidator = new UpsertCrmDefaultCustomerCommandValidator();
        var customerResult = await customerValidator.ValidateAsync(
            new UpsertCrmDefaultCustomerCommand(new CrmDefaultCustomerUpsertDto(0, null, null, null, false, false)));

        customerResult.IsValid.Should().BeFalse();
        customerResult.Errors.Should().Contain(e => e.PropertyName == "Dto.DefaultPaymentTermId");

        var locationValidator = new UpsertCrmDefaultServiceLocationCommandValidator();
        var locationResult = await locationValidator.ValidateAsync(
            new UpsertCrmDefaultServiceLocationCommand(
                new CrmDefaultServiceLocationUpsertDto(null, -2, null, null, true, null, null, null, null, true, false)));

        locationResult.IsValid.Should().BeFalse();
        locationResult.Errors.Should().Contain(e => e.PropertyName == "Dto.DefaultMaterialPricingMatrixId");
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
    public async Task AddNoteValidator_RequiresNoteTextAndDefinedNoteType()
    {
        var validator = new AddCrmCustomerNoteCommandValidator();
        var missingText = new AddCrmCustomerNoteCommand(1, new CrmNoteCreateDto(""));
        var badType = new AddCrmCustomerNoteCommand(1, new CrmNoteCreateDto("ok", NoteTypeId: 99));
        var valid = new AddCrmCustomerNoteCommand(1, new CrmNoteCreateDto("ok"));

        var missingTextResult = await validator.ValidateAsync(missingText);
        var badTypeResult = await validator.ValidateAsync(badType);
        var validResult = await validator.ValidateAsync(valid);

        missingTextResult.IsValid.Should().BeFalse();
        missingTextResult.Errors.Should().Contain(e => e.PropertyName == "Dto.NoteText");
        badTypeResult.IsValid.Should().BeFalse();
        badTypeResult.Errors.Should().Contain(e => e.PropertyName == "Dto.NoteTypeId");
        validResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAndPatchNoteValidators_RejectEmptyNoteTextAndBadType()
    {
        var updateValidator = new UpdateCrmCustomerNoteCommandValidator();
        var patchValidator = new PatchCrmCustomerNoteCommandValidator();

        var emptyUpdate = new UpdateCrmCustomerNoteCommand(
            1,
            2,
            new CrmNoteUpdateDto("", "t", (short)CrmNoteType.General, 1, false));
        var badUpdateType = new UpdateCrmCustomerNoteCommand(
            1,
            2,
            new CrmNoteUpdateDto("ok", null, 9, 1, false));
        var emptyPatch = new PatchCrmCustomerNoteCommand(1, 2, new CrmNotePatchDto(NoteText: " "));
        var badPatchType = new PatchCrmCustomerNoteCommand(1, 2, new CrmNotePatchDto(NoteTypeId: 0));
        var validPatch = new PatchCrmCustomerNoteCommand(1, 2, new CrmNotePatchDto(IsPinned: true));

        (await updateValidator.ValidateAsync(emptyUpdate)).IsValid.Should().BeFalse();
        (await updateValidator.ValidateAsync(badUpdateType)).IsValid.Should().BeFalse();
        (await patchValidator.ValidateAsync(emptyPatch)).IsValid.Should().BeFalse();
        (await patchValidator.ValidateAsync(badPatchType)).IsValid.Should().BeFalse();
        (await patchValidator.ValidateAsync(validPatch)).IsValid.Should().BeTrue();
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
    public async Task ServiceLocationValidator_BreakIdsMustBePositiveWhenSet()
    {
        var createValidator = new AddCrmCustomerServiceLocationCommandValidator();
        var updateValidator = new UpdateCrmCustomerServiceLocationCommandValidator();
        var zero = new AddCrmCustomerServiceLocationCommand(1, ValidLocation() with { Break1Id = 0 });
        var negative = new AddCrmCustomerServiceLocationCommand(1, ValidLocation() with { Break2Id = -5 });
        var omitted = new AddCrmCustomerServiceLocationCommand(1, ValidLocation());
        var positive = new AddCrmCustomerServiceLocationCommand(
            1,
            ValidLocation() with { Break1Id = 8, Break2Id = 9 });
        var updateZero = new UpdateCrmCustomerServiceLocationCommand(
            1,
            2,
            ValidLocationUpdate() with { Break1Id = 0, Break2Id = null });
        var updateClear = new UpdateCrmCustomerServiceLocationCommand(
            1,
            2,
            ValidLocationUpdate() with { Break1Id = null, Break2Id = null });

        var zeroResult = await createValidator.ValidateAsync(zero);
        var negativeResult = await createValidator.ValidateAsync(negative);
        var omittedResult = await createValidator.ValidateAsync(omitted);
        var positiveResult = await createValidator.ValidateAsync(positive);
        var updateZeroResult = await updateValidator.ValidateAsync(updateZero);
        var updateClearResult = await updateValidator.ValidateAsync(updateClear);

        zeroResult.IsValid.Should().BeFalse();
        zeroResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Break1Id");
        negativeResult.IsValid.Should().BeFalse();
        negativeResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Break2Id");
        omittedResult.IsValid.Should().BeTrue();
        positiveResult.IsValid.Should().BeTrue();
        updateZeroResult.IsValid.Should().BeFalse();
        updateZeroResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Break1Id");
        updateClearResult.IsValid.Should().BeTrue();
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
    public async Task CreateValidator_LocationAddressRequiresLine1PostalStateAndCountry()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var missingAddress = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Residential,
                "Main",
                "Main site",
                AddressLine2: "Suite 2",
                City: "Austin")
        });
        var optionalBlank = new CreateCrmCustomerCommand(ValidCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = ValidLocation() with
            {
                AddressLine2 = " ",
                AddressLine3 = null,
                AddressLine4 = "",
                City = null,
                County = null
            }
        });

        var missingResult = await validator.ValidateAsync(missingAddress);
        var optionalResult = await validator.ValidateAsync(optionalBlank);

        missingResult.IsValid.Should().BeFalse();
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.AddressLine1");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.PostalCode");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.State");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.ServiceLocation.Country");
        missingResult.Errors.Should().NotContain(e => e.PropertyName == "Dto.ServiceLocation.AddressLine2");
        missingResult.Errors.Should().NotContain(e => e.PropertyName == "Dto.ServiceLocation.City");
        optionalResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateServiceLocationValidator_RequiresNameDisplayNameAddressAndContactName()
    {
        var validator = new UpdateCrmCustomerServiceLocationCommandValidator();
        var missing = new UpdateCrmCustomerServiceLocationCommand(
            1,
            2,
            new CrmServiceLocationUpdateDto(ServiceLocationType.Commercial));
        var missingContactName = new UpdateCrmCustomerServiceLocationCommand(
            1,
            2,
            ValidLocationUpdate() with { PrimaryContactPhone = "+1 555-0100" });
        var optionalAddress = new UpdateCrmCustomerServiceLocationCommand(
            1,
            2,
            ValidLocationUpdate() with
            {
                AddressLine2 = null,
                AddressLine3 = " ",
                AddressLine4 = "",
                City = null,
                County = null
            });

        var missingResult = await validator.ValidateAsync(missing);
        var contactResult = await validator.ValidateAsync(missingContactName);
        var optionalResult = await validator.ValidateAsync(optionalAddress);

        missingResult.IsValid.Should().BeFalse();
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.DisplayName");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.AddressLine1");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.PostalCode");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.State");
        missingResult.Errors.Should().Contain(e => e.PropertyName == "Dto.Country");
        missingResult.Errors.Should().NotContain(e => e.PropertyName == "Dto.City");
        contactResult.IsValid.Should().BeFalse();
        contactResult.Errors.Should().Contain(e => e.PropertyName == "Dto.PrimaryContactName");
        optionalResult.IsValid.Should().BeTrue();
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
        new(
            ServiceLocationType.Residential,
            "Main",
            "Main site",
            AddressLine1: "100 Main St",
            State: "TX",
            Country: "US",
            PostalCode: "78701");

    private static CrmServiceLocationUpdateDto ValidLocationUpdate() =>
        new(
            ServiceLocationType.Residential,
            "Main",
            "Main site",
            AddressLine1: "100 Main St",
            State: "TX",
            Country: "US",
            PostalCode: "78701",
            IsActive: true);
}
