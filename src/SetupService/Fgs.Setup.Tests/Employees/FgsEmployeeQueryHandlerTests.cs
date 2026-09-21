using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
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

        var handler = CreateListHandler(readRepository);
        var response = await handler.Handle(
            new ListEmployeesQuery(new SetupListQuery(), new FgsEmployeeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        readRepository.Verify(
            r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsEmployeeListFilters>(), It.IsAny<CancellationToken>()),
            Times.Once);
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

        var handler = CreateListHandler(readRepository);
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

    [Fact]
    public async Task List_WithTechFiltersAndSearch_PassesThroughToRepository()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<SetupListQuery>(),
                It.IsAny<FgsEmployeeListFilters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsEmployeeSummaryDto>([], 1, 25, 0));

        var handler = CreateListHandler(readRepository);
        await handler.Handle(
            new ListEmployeesQuery(
                new SetupListQuery(Search: "555"),
                new FgsEmployeeListFilters(
                    TechTradeIds: [1, 2],
                    TechSkillIds: [3],
                    DispatchZoneIds: [4, 5])),
            CancellationToken.None);

        readRepository.Verify(
            r => r.ListAsync(
                It.Is<SetupListQuery>(q => q.Search == "555"),
                It.Is<FgsEmployeeListFilters>(f =>
                    f.TechTradeIds != null
                    && f.TechTradeIds.SequenceEqual(new long[] { 1L, 2L })
                    && f.TechSkillIds != null
                    && f.TechSkillIds.SequenceEqual(new long[] { 3L })
                    && f.DispatchZoneIds != null
                    && f.DispatchZoneIds.SequenceEqual(new long[] { 4L, 5L })
                    && f.UserIds == null),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task List_WithRoleIds_CallsUserClientAndPassesUserIds()
    {
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<SetupListQuery>(),
                It.IsAny<FgsEmployeeListFilters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsEmployeeSummaryDto>([], 1, 25, 0));

        var userClient = new Mock<IUserInternalUsersClient>();
        userClient
            .Setup(c => c.GetUserIdsByRolesAsync(
                It.IsAny<IEnumerable<long>>(),
                "10",
                "20",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<IReadOnlyList<Guid>>.Ok([userId]));

        var handler = CreateListHandler(readRepository, userClient);
        await handler.Handle(
            new ListEmployeesQuery(
                new SetupListQuery(),
                new FgsEmployeeListFilters(RoleIds: [7, 8])),
            CancellationToken.None);

        userClient.Verify(
            c => c.GetUserIdsByRolesAsync(
                It.Is<IEnumerable<long>>(ids => ids.SequenceEqual(new long[] { 7L, 8L })),
                "10",
                "20",
                It.IsAny<CancellationToken>()),
            Times.Once);

        readRepository.Verify(
            r => r.ListAsync(
                It.IsAny<SetupListQuery>(),
                It.Is<FgsEmployeeListFilters>(f =>
                    f.UserIds != null
                    && f.UserIds.SequenceEqual(new Guid[] { userId })
                    && f.RoleIds != null
                    && f.RoleIds.SequenceEqual(new long[] { 7L, 8L })),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task List_WithRoleIds_WhenNoMatchingUsers_ReturnsEmptyPageWithoutRepository()
    {
        var readRepository = new Mock<IFgsEmployeeReadRepository>();
        var userClient = new Mock<IUserInternalUsersClient>();
        userClient
            .Setup(c => c.GetUserIdsByRolesAsync(
                It.IsAny<IEnumerable<long>>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<IReadOnlyList<Guid>>.Ok([]));

        var handler = CreateListHandler(readRepository, userClient);
        var response = await handler.Handle(
            new ListEmployeesQuery(
                new SetupListQuery(Page: 2, PageSize: 10),
                new FgsEmployeeListFilters(RoleIds: [9])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().BeEmpty();
        response.Data.Page.Should().Be(2);
        response.Data.PageSize.Should().Be(10);
        response.Data.TotalCount.Should().Be(0);
        readRepository.Verify(
            r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsEmployeeListFilters>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static ListEmployeesQueryHandler CreateListHandler(
        Mock<IFgsEmployeeReadRepository> readRepository,
        Mock<IUserInternalUsersClient>? userClient = null)
    {
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        return new ListEmployeesQueryHandler(
            readRepository.Object,
            (userClient ?? new Mock<IUserInternalUsersClient>()).Object,
            tenantAccessor.Object);
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
            null,
            new FgsEmployeeTechnicianProfileDetailDto(
                10,
                "T-001",
                "Alex",
                true,
                8m,
                null,
                StartLocationTypeIds.Office,
                new TimeOnly(8, 0),
                1,
                2,
                null,
                "+15559876543",
                "Mobile bio"));
}
