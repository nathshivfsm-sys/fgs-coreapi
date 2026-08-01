using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Application.Features.Employees.Queries.GetFgsEmployeeById;
using Fgs.Setup.Application.Features.Employees.Queries.ListEmployees;
using Fgs.Setup.Domain.Entities;
using Moq;

namespace Fgs.Setup.Tests.Employees;

public sealed class FgsEmployeeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = CreateDetail(1);

        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsEmployeeByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsEmployeeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsEmployeeDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsEmployeeByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsEmployeeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsEmployeeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsEmployeeSummaryDto>([], 1, 25, 0));

        var handler = new ListEmployeesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListEmployeesQuery(new SetupListQuery(), new FgsEmployeeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task List_WithStatusFilter_PassesFilterToRepository()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<SetupListQuery>(),
                It.Is<FgsEmployeeListFilters>(f => f.StatusId == EmployeeStatusIds.Active),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsEmployeeSummaryDto>([], 1, 25, 0));

        var handler = new ListEmployeesQueryHandler(readRepository.Object);
        await handler.Handle(
            new ListEmployeesQuery(
                new SetupListQuery(),
                new FgsEmployeeListFilters(StatusId: EmployeeStatusIds.Active)),
            CancellationToken.None);

        readRepository.Verify(
            r => r.ListAsync(
                It.IsAny<SetupListQuery>(),
                It.Is<FgsEmployeeListFilters>(f => f.StatusId == EmployeeStatusIds.Active),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static FgsEmployeeDetailDto CreateDetail(long id) =>
        new(
            id,
            null,
            "EMP-001",
            EmployeeTypeIds.Technician,
            "Alex Tech",
            "Alex",
            null,
            "Tech",
            null,
            new DateOnly(2026, 1, 15),
            null,
            EmployeeStatusIds.Active,
            null,
            "alex@example.com",
            null,
            "+15551234567",
            new FgsEmployeeAddressDetailDto(
                Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                "100 Main St",
                "Apt 2",
                "Austin",
                "TX",
                "US",
                "78701"),
            null,
            40m,
            60m,
            80m,
            LaborBurdenTypeIds.Percentage,
            25m,
            false,
            null);
}
