using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Commands.CreateFgsVendor;
using Fgs.Setup.Application.Features.Vendors.Commands.PatchFgsVendor;
using Fgs.Setup.Application.Features.Vendors.Commands.UpdateFgsVendor;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using Fgs.Setup.Application.Features.Vendors.Validators;
using Moq;

namespace Fgs.Setup.Tests.Vendors;

public sealed class FgsVendorValidatorTests
{
    private readonly Mock<IFgsVendorReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenVendorCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsVendorCommandValidator(_readRepository.Object);
        var command = new CreateFgsVendorCommand(new FgsVendorCreateDto("", "Name", "LegalName", "VendorType", null, "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes value", false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.VendorCode");
    }

    [Fact]
    public async Task CreateValidator_WhenVendorCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsVendorCommandValidator(_readRepository.Object);
        var args = new FgsVendorCreateDto("TEST", "Name", "LegalName", "VendorType", null, "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes value", false);
        var command = new CreateFgsVendorCommand(args with { VendorCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.VendorCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByVendorCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsPaymentTermIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsVendorCommandValidator(_readRepository.Object);
        var command = new UpdateFgsVendorCommand(5, new FgsVendorUpdateDto("TEST", "Name", "LegalName", "VendorType", null, "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes value", false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
