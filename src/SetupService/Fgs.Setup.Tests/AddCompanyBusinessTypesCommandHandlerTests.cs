using Fgs.Contracts.Clients;
using Fgs.Setup.Application.Abstractions.Tenants;
using Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class AddCompanyBusinessTypesCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsCompanyBusinessTypeService()
    {
        var service = new Mock<ICompanyBusinessTypeService>();
        var request = new AddCompanyBusinessTypesRequest([1, 2], Guid.NewGuid(), "ACME", "Acme Co");
        var handler = new AddCompanyBusinessTypesCommandHandler(service.Object);

        var response = await handler.Handle(
            new AddCompanyBusinessTypesCommand(10, 1, request),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        service.Verify(
            s => s.AddCompanyBusinessTypesAsync(10, 1, request, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
