using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Commands.CreateFgsSetupTaxDetail;
using Fgs.Setup.Application.Features.SetupTaxDetails.Commands.PatchFgsSetupTaxDetail;
using Fgs.Setup.Application.Features.SetupTaxDetails.Commands.UpdateFgsSetupTaxDetail;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using Fgs.Setup.Application.Features.SetupTaxDetails.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxDetails;

public sealed class FgsSetupTaxDetailValidatorTests
{
    private readonly Mock<IFgsSetupTaxDetailReadRepository> _readRepository = new();

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsTaxIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsTaxAuthorityIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsSetupTaxDetailCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupTaxDetailCommand(5, new FgsSetupTaxDetailUpdateDto(1, 1, DateOnly.FromDateTime(DateTime.UtcNow), null, 10.5m, false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
