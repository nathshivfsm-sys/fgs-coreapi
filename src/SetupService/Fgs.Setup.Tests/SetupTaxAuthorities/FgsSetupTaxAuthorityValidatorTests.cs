using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.CreateFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.PatchFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.UpdateFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxAuthorities;

public sealed class FgsSetupTaxAuthorityValidatorTests
{
    private readonly Mock<IFgsSetupTaxAuthorityReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupTaxAuthorityCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupTaxAuthorityCommand(new FgsSetupTaxAuthorityCreateDto("", "Name value", "TEST", false, 10.5m, "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSetupTaxAuthorityCommandValidator(_readRepository.Object);
        var args = new FgsSetupTaxAuthorityCreateDto("TEST", "Name value", "TEST", false, 10.5m, "Description value");
        var command = new CreateFgsSetupTaxAuthorityCommand(args with { Code = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSetupTaxAuthorityCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupTaxAuthorityCommand(5, new FgsSetupTaxAuthorityUpdateDto("TEST", "Name value", "TEST", false, 10.5m, "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
