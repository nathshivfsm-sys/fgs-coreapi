using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Queries;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class ListSetupEntitiesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsPagedBillingCategories()
    {
        var entityRegistry = CreateEntityRegistry(FgsBillingCategoryDescriptor.Create());

        var summary = new FgsBillingCategorySummaryDto(
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
            UpdatedOn: null,
            IsActive: true);

        var readRepository = new Mock<IEntityReadRepository>();
        readRepository
            .Setup(repository => repository.ListAsync(
                It.IsAny<CatalogEntityDescriptor>(),
                It.IsAny<PagedQuery>(),
                It.IsAny<IReadOnlyDictionary<string, string?>>(),
                typeof(FgsBillingCategorySummaryDto),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<object>([summary], 1, 25, 1));

        var handler = new ListCatalogEntitiesQueryHandler<FgsBillingCategorySummaryDto>(entityRegistry, readRepository.Object);

        var response = await handler.Handle(
            new ListCatalogEntitiesQuery<FgsBillingCategorySummaryDto>(
                EntityKeys.BillingCategory,
                new PagedQuery(Page: 1, PageSize: 25, Search: "Labor")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
        response.Data.TotalCount.Should().Be(1);
    }

    private static IEntityRegistry CreateEntityRegistry(CatalogEntityDescriptor descriptor)
    {
        var registry = new EntityRegistry();
        registry.Register(descriptor);
        return registry;
    }
}
