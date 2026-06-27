using Fgs.Contracts.Clients;
using Fgs.Credentials.Options;
using Fgs.Setup.Application.Abstractions.Tenants;
using Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class AddCompanyBusinessTypesCommandHandlerTests
{
    private const string InternalServiceKey = "test-internal-key";

    [Fact]
    public async Task Handle_WithValidInternalServiceKey_CallsCompanyBusinessTypeService()
    {
        var service = new Mock<ICompanyBusinessTypeService>();
        var request = new AddCompanyBusinessTypesRequest([1, 2], Guid.NewGuid(), "ACME", "Acme Co");
        var handler = CreateHandler(service.Object);

        var response = await handler.Handle(
            new AddCompanyBusinessTypesCommand(10, 1, request, InternalServiceKey),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        service.Verify(
            s => s.AddCompanyBusinessTypesAsync(10, 1, request, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidInternalServiceKey_ReturnsUnauthorized()
    {
        var service = new Mock<ICompanyBusinessTypeService>();
        var request = new AddCompanyBusinessTypesRequest([1], Guid.NewGuid(), "ACME", "Acme Co");
        var handler = CreateHandler(service.Object);

        var response = await handler.Handle(
            new AddCompanyBusinessTypesCommand(10, 1, request, "wrong-key"),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(401);
        service.Verify(
            s => s.AddCompanyBusinessTypesAsync(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<AddCompanyBusinessTypesRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static AddCompanyBusinessTypesCommandHandler CreateHandler(ICompanyBusinessTypeService service) =>
        new(
            service,
            Options.Create(new CredentialDistributionOptions
            {
                InternalServiceKey = InternalServiceKey
            }));
}
