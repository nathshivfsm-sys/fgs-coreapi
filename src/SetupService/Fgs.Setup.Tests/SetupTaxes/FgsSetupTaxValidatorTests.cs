using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.CreateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.PatchFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.UpdateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using Fgs.Setup.Application.Features.SetupTaxes.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxes;

public sealed class FgsSetupTaxValidatorTests
{
    private readonly Mock<IFgsSetupTaxReadRepository> _readRepository = new();
    private readonly Mock<IFgsSetupTaxAuthorityReadRepository> _taxAuthorityReadRepository = new();

    public FgsSetupTaxValidatorTests()
    {
        _taxAuthorityReadRepository
            .Setup(r => r.ExistsByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    [Fact]
    public async Task CreateValidator_WhenTaxCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupTaxCommandValidator(_readRepository.Object, _taxAuthorityReadRepository.Object);
        var command = new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto("", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TaxCode");
    }

    [Fact]
    public async Task CreateValidator_WhenTaxCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSetupTaxCommandValidator(_readRepository.Object, _taxAuthorityReadRepository.Object);
        var args = new FgsSetupTaxCreateDto("TEST", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value");
        var command = new CreateFgsSetupTaxCommand(args with { TaxCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TaxCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByTaxCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSetupTaxCommandValidator(_readRepository.Object, _taxAuthorityReadRepository.Object);
        var command = new UpdateFgsSetupTaxCommand(5, new FgsSetupTaxUpdateDto("TEST", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
