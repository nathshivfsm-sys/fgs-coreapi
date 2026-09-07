using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using Fgs.Inventory.Application.Features.InventoryCategories.Queries.ListInventoryCategoriesWithSubCategories;
using Moq;

namespace Fgs.Inventory.Tests.InventoryCategories;

public sealed class ListInventoryCategoriesWithSubCategoriesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsNestedCategories()
    {
        var read = new Mock<IFgsInventoryCategoryReadRepository>();
        var nested = new FgsInventoryCategoryWithSubCategoriesDto(
            1,
            "PARTS",
            "Parts",
            "desc",
            null,
            null,
            null,
            1,
            false,
            true,
            [
                new FgsInventorySubCategoryNestedDto(10, "FILTERS", "Filters", null, 1, false, true)
            ]);
        read.Setup(r => r.ListWithSubCategoriesAsync(
                It.IsAny<InventoryListQuery>(),
                It.IsAny<FgsInventoryCategoryListFilters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsInventoryCategoryWithSubCategoriesDto>([nested], 1, 25, 1));

        var handler = new ListInventoryCategoriesWithSubCategoriesQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListInventoryCategoriesWithSubCategoriesQuery(
                new InventoryListQuery(),
                new FgsInventoryCategoryListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
        response.Data.Items[0].SubCategories.Should().ContainSingle(s => s.Name == "Filters");
    }
}
