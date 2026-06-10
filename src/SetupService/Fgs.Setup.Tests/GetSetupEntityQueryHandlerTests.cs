using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Queries;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class GetSetupEntityQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenEntityExists_ReturnsDetailDto()
    {
        var entityRegistry = CreateEntityRegistry(FgsBillingCategoryDescriptor.Create());

        var detail = new FgsBillingCategoryDetailDto(
            Id: 1,
            TenantId: 10,
            CompanyId: 20,
            BillingCategoryType: "LB",
            BillingCategoryName: "Labor",
            Description: null,
            DisplayOrder: 1,
            IsSystemDefined: false,
            ShowToFieldTech: false,
            AllowToPick: true,
            CreatedOn: DateTimeOffset.UtcNow,
            CreatedBy: null,
            UpdatedOn: null,
            UpdatedBy: null,
            IsActive: true);

        var readRepository = new Mock<IEntityReadRepository>();
        readRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<CatalogEntityDescriptor>(), "1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var handler = new GetCatalogEntityQueryHandler<FgsBillingCategoryDetailDto>(entityRegistry, readRepository.Object);

        var response = await handler.Handle(
            new GetCatalogEntityQuery<FgsBillingCategoryDetailDto>(EntityKeys.BillingCategory, "1"),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.BillingCategoryName.Should().Be("Labor");
    }

    [Fact]
    public async Task Handle_WhenEntityMissing_ReturnsNotFound()
    {
        var entityRegistry = CreateEntityRegistry(FgsBillingCategoryDescriptor.Create());

        var readRepository = new Mock<IEntityReadRepository>();
        readRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<CatalogEntityDescriptor>(), "99", It.IsAny<CancellationToken>()))
            .ReturnsAsync((object?)null);

        var handler = new GetCatalogEntityQueryHandler<FgsBillingCategoryDetailDto>(entityRegistry, readRepository.Object);

        var response = await handler.Handle(
            new GetCatalogEntityQuery<FgsBillingCategoryDetailDto>(EntityKeys.BillingCategory, "99"),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    private static IEntityRegistry CreateEntityRegistry(CatalogEntityDescriptor descriptor)
    {
        var registry = new EntityRegistry();
        registry.Register(descriptor);
        return registry;
    }
}
