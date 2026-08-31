using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.ApiClients.Dtos;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using Fgs.User.Application.Features.Permissions.Dtos;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Tests.Application;

public sealed class ApplicationDtosCoverageTests
{
    [Fact]
    public void Dtos_SupportWithExpressionsAndPropertyAccess()
    {
        var apiClient = new FgsApiClientDetailDto(1, Guid.NewGuid(), "App", "d", "c", "e", 60, true);
        (apiClient with { IsActive = false }).ApplicationName.Should().Be("App");

        var permission = new FgsPermissionDetailDto(1, "A.B", "M", "R", "X", "N", "d", 1, true);
        (permission with { Module = "M2" }).PermissionCode.Should().Be("A.B");

        var dataAccess = new FgsDataAccessDetailDto(1, "CODE", "Name", "d", true, 1, true);
        (dataAccess with { IsBuiltIn = false }).Name.Should().Be("Name");

        var apiEvent = new FgsApiEventDetailDto(1, "E", "C", "N", "d", 1, 1, true);
        (apiEvent with { EventVersion = 2 }).EventCode.Should().Be("E");

        var webhook = new FgsApiWebhookDetailDto(1, "N", "d", "url", "NONE", null, null, 30, 5, null, true);
        (webhook with { TimeoutSeconds = 60 }).Name.Should().Be("N");

        var subscription = new FgsApiWebhookSubscriptionDetailDto(1, 1, 2, DateTimeOffset.UtcNow, "test");
        (subscription with { FgsApiEventId = 3 }).FgsApiWebhookId.Should().Be(1);

        var secret = new FgsApiSecretDetailDto(1, 1, "S", null, null, null, null, true, DateTimeOffset.UtcNow, "t");
        (secret with { Name = "S2" }).FgsApiClientId.Should().Be(1);

        var scope = new FgsDataAccessScopeDetailDto(1, 1, "T", "IN", "v", 1);
        (scope with { ScopeValue = "v2" }).ScopeType.Should().Be("T");

        var role = new FgsRoleDetailDto(1, "R", "N", "d", null, false, 1, true);
        (role with { IsBuiltIn = true }).RoleCode.Should().Be("R");

        var userDto = new FgsUserDetailDto(Guid.NewGuid(), "N", "e", null, 1, "R", "P", true, true);
        (userDto with { IsActive = false }).Email.Should().Be("e");

        var endpoint = new FgsPublicEndpointDetailDto(1, "API", "PROD", "url", "d", true);
        (endpoint with { IsActive = false }).EndpointType.Should().Be("API");

        var tenant = new TenantDetailDto(1, Guid.NewGuid(), "C", "N", null, null, null, null, null, null, 1, "b", true);
        (tenant with { Name = "N2" }).Code.Should().Be("C");

        var company = new CompanyDetailDto(1, 1, 1, Guid.NewGuid(), "C", "N", null, null, null, null, null, null, null, true, null, null);
        (company with { Name = "N2" }).Code.Should().Be("C");

        var serviceSetup = new FgsTenantServiceSetupDetailDto(
            1, 1, TimeCardOption.None, null, false, false, false, false, false, false,
            null, null, null, null, null, "ARRIVE", false, false, 1, 1, 1, 1,
            null, null, null, null, null, EstimateRevisionCreationModes.OnDemand, true);
        (serviceSetup with { EnableCustomerPortal = true }).TenantId.Should().Be(1);

        var serviceAccounts = new FgsTenantServiceAccountsSetupDetailDto(1, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, true);
        (serviceAccounts with { IsActive = false }).BankAccountId.Should().Be(1);

        var rolePermission = new FgsRolePermissionDetailDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        (rolePermission with { FgsPermissionId = 3 }).FgsRoleId.Should().Be(1);

        var roleMenu = new FgsRoleMenuDetailDto(1, 10, 100, 1, true, DateTimeOffset.UtcNow, "t");
        roleMenu.Id.Should().Be(1);
        roleMenu.RoleId.Should().Be(10);
        roleMenu.MenuId.Should().Be(100);
        roleMenu.DisplayOrder.Should().Be(1);
        roleMenu.IsActive.Should().BeTrue();
        roleMenu.CreatedBy.Should().Be("t");
        (roleMenu with { DisplayOrder = 2 }).DisplayOrder.Should().Be(2);
        var roleMenuSync = new FgsRoleMenuSyncDto(10, [new FgsRoleMenuSyncItemDto(100, 1, true)]);
        (roleMenuSync with { RoleId = 11 }).Items.Should().ContainSingle(x => x.MenuId == 100);

        var tenantMenu = new FgsTenantMenuDetailDto(1, 100, 1, true, DateTimeOffset.UtcNow, "t");
        tenantMenu.Id.Should().Be(1);
        tenantMenu.MenuId.Should().Be(100);
        tenantMenu.DisplayOrder.Should().Be(1);
        tenantMenu.IsActive.Should().BeTrue();
        tenantMenu.CreatedBy.Should().Be("t");
        (tenantMenu with { IsActive = false }).IsActive.Should().BeFalse();
        var tenantMenuSync = new FgsTenantMenuSyncDto([new FgsTenantMenuSyncItemDto(100, 1)]);
        (tenantMenuSync with { Items = [new FgsTenantMenuSyncItemDto(101, 2)] }).Items
            .Should().ContainSingle(x => x.MenuId == 101);

        var rolePermissionSummary = new FgsRolePermissionSummaryDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        (rolePermissionSummary with { FgsPermissionId = 3 }).FgsRoleId.Should().Be(1);
        rolePermissionSummary.Id.Should().Be(1);
        rolePermissionSummary.CreatedBy.Should().Be("t");

        var roleDataAccessSummary = new FgsRoleDataAccessSummaryDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        (roleDataAccessSummary with { FgsDataAccessId = 3 }).FgsRoleId.Should().Be(1);
        roleDataAccessSummary.Id.Should().Be(1);
        roleDataAccessSummary.CreatedBy.Should().Be("t");

        var userRoleSummary = new FgsUserRoleSummaryDto(1, Guid.NewGuid(), 2, DateTimeOffset.UtcNow, "t");
        (userRoleSummary with { FgsRoleId = 3 }).CreatedBy.Should().Be("t");
        userRoleSummary.Id.Should().Be(1);

        var roleDataAccess = new FgsRoleDataAccessDetailDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        (roleDataAccess with { FgsDataAccessId = 3 }).FgsRoleId.Should().Be(1);

        var userRole = new FgsUserRoleDetailDto(1, Guid.NewGuid(), 2, DateTimeOffset.UtcNow, "t");
        (userRole with { FgsRoleId = 3 }).CreatedBy.Should().Be("t");

        var location = new LocationDetailDto(Guid.NewGuid(), "1", null, null, null, "Austin", "TX", null, "US", "78701", null, null, null, null, true);
        (location with { City = "Dallas" }).State.Should().Be("TX");

        var filters = new FgsApiClientListFilters("a", "e");
        filters.ApplicationName.Should().Be("a");

        var patchSetup = new FgsTenantServiceSetupPatchDto(EnableCustomerPortal: true, BillToStartNumber: 200);
        patchSetup.EnableCustomerPortal.Should().BeTrue();
    }
}
