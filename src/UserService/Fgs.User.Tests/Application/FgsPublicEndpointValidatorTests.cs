using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Commands.CreateFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using Fgs.User.Application.Features.PublicEndpoints.Validators;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class FgsPublicEndpointValidatorTests
{
    [Fact]
    public async Task Create_WithDuplicateTypeAndEnvironment_Fails()
    {
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        read
            .Setup(r => r.ExistsByTypeAndEnvironmentAsync("BFF", "PROD", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsPublicEndpointCommandValidator(read.Object);
        var result = await validator.ValidateAsync(new CreateFgsPublicEndpointCommand(
            new FgsPublicEndpointCreateDto("BFF", "PROD", "https://api.example.com", "Prod BFF")));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("already exists", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Create_WithValidPayload_Succeeds()
    {
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        read
            .Setup(r => r.ExistsByTypeAndEnvironmentAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsPublicEndpointCommandValidator(read.Object);
        var result = await validator.ValidateAsync(new CreateFgsPublicEndpointCommand(
            new FgsPublicEndpointCreateDto("API", "SANDBOX", "https://sandbox.example.com/api", null)));

        result.IsValid.Should().BeTrue();
    }
}
