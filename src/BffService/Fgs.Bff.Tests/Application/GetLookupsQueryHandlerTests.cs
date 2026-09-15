using Fgs.Bff.Application.Features.Lookups;
using Fgs.Bff.Application.Features.Lookups.Abstractions;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Bff.Application.Features.Lookups.Queries.GetLookups;
using Moq;

namespace Fgs.Bff.Tests.Application;

public sealed class GetLookupsQueryHandlerTests
{
    [Fact]
    public async Task Handle_MultipleKeys_ReturnsAllResults()
    {
        var gateway = new Mock<ILookupGateway>();
        gateway
            .Setup(g => g.GetLookupAsync(
                It.Is<LookupRequestDto>(r => r.Key == LookupKey.GloCountry),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LookupResultDto(
                LookupKey.GloCountry,
                [new LookupItemDto("US", "US", "United States", null, null)]));

        gateway
            .Setup(g => g.GetLookupAsync(
                It.Is<LookupRequestDto>(r => r.Key == LookupKey.TechTrade),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LookupResultDto(
                LookupKey.TechTrade,
                [new LookupItemDto("1", "HVAC", "HVAC", 1, null)]));

        var handler = new GetLookupsQueryHandler(gateway.Object);
        var response = await handler.Handle(
            new GetLookupsQuery(
            [
                new LookupRequestDto(LookupKey.GloCountry),
                new LookupRequestDto(LookupKey.TechTrade)
            ]),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
        response.Data!.Select(r => r.Key).Should().BeEquivalentTo(
            [LookupKey.GloCountry, LookupKey.TechTrade]);
        response.Data.Should().OnlyContain(r => r.Error == null);
    }

    [Fact]
    public async Task Handle_PartialFailure_SetsErrorOnFailedKeyOnly()
    {
        var gateway = new Mock<ILookupGateway>();
        gateway
            .Setup(g => g.GetLookupAsync(
                It.Is<LookupRequestDto>(r => r.Key == LookupKey.GloCountry),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LookupResultDto(
                LookupKey.GloCountry,
                [new LookupItemDto("US", "US", "United States", null, null)]));

        gateway
            .Setup(g => g.GetLookupAsync(
                It.Is<LookupRequestDto>(r => r.Key == LookupKey.Customer),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LookupResultDto(LookupKey.Customer, [], "CRM unavailable"));

        var handler = new GetLookupsQueryHandler(gateway.Object);
        var response = await handler.Handle(
            new GetLookupsQuery(
            [
                new LookupRequestDto(LookupKey.GloCountry),
                new LookupRequestDto(LookupKey.Customer)
            ]),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        var country = response.Data!.Single(r => r.Key == LookupKey.GloCountry);
        var customer = response.Data!.Single(r => r.Key == LookupKey.Customer);
        country.Error.Should().BeNull();
        country.Items.Should().NotBeEmpty();
        customer.Error.Should().Be("CRM unavailable");
        customer.Items.Should().BeEmpty();
    }
}
