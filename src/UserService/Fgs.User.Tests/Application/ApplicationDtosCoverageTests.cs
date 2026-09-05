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

        var role = new FgsRoleDetailDto(1, "R", "N", "d", null, false, 1, true);
        role.Id.Should().Be(1);
        role.RoleCode.Should().Be("R");
        role.Name.Should().Be("N");
        role.Description.Should().Be("d");
        role.ParentRoleId.Should().BeNull();
        role.IsBuiltIn.Should().BeFalse();
        role.DisplayOrder.Should().Be(1);
        role.IsActive.Should().BeTrue();
        (role with { IsBuiltIn = true }).RoleCode.Should().Be("R");
        var roleSummary = new FgsRoleSummaryDto(1, "R", "N", "d", 2, true, 3, false);
        roleSummary.Id.Should().Be(1);
        roleSummary.RoleCode.Should().Be("R");
        roleSummary.Name.Should().Be("N");
        roleSummary.Description.Should().Be("d");
        roleSummary.ParentRoleId.Should().Be(2);
        roleSummary.IsBuiltIn.Should().BeTrue();
        roleSummary.DisplayOrder.Should().Be(3);
        roleSummary.IsActive.Should().BeFalse();
        var roleLookup = new FgsRoleLookupDto(1, "R", "N", true, 1);
        roleLookup.Id.Should().Be(1);
        roleLookup.RoleCode.Should().Be("R");
        roleLookup.Name.Should().Be("N");
        roleLookup.IsBuiltIn.Should().BeTrue();
        roleLookup.DisplayOrder.Should().Be(1);
        var roleFilters = new FgsRoleListFilters("R", "N", true);
        roleFilters.RoleCode.Should().Be("R");
        roleFilters.Name.Should().Be("N");
        roleFilters.IsBuiltIn.Should().BeTrue();

        var permission = new FgsPermissionDetailDto(1, "A.B", "M", "R", "X", "N", "d", 1, true);
        permission.Id.Should().Be(1);
        permission.PermissionCode.Should().Be("A.B");
        permission.Module.Should().Be("M");
        permission.Resource.Should().Be("R");
        permission.Action.Should().Be("X");
        permission.Name.Should().Be("N");
        permission.Description.Should().Be("d");
        permission.DisplayOrder.Should().Be(1);
        permission.IsActive.Should().BeTrue();
        (permission with { Module = "M2" }).PermissionCode.Should().Be("A.B");
        var permissionSummary = new FgsPermissionSummaryDto(1, "A.B", "M", "R", "X", "N", "d", 1, true);
        permissionSummary.Id.Should().Be(1);
        permissionSummary.PermissionCode.Should().Be("A.B");
        permissionSummary.Module.Should().Be("M");
        permissionSummary.Resource.Should().Be("R");
        permissionSummary.Action.Should().Be("X");
        permissionSummary.Name.Should().Be("N");
        permissionSummary.Description.Should().Be("d");
        permissionSummary.DisplayOrder.Should().Be(1);
        permissionSummary.IsActive.Should().BeTrue();
        var permissionLookup = new FgsPermissionLookupDto(1, "A.B", "M", "R", "X", "N", 1);
        permissionLookup.Id.Should().Be(1);
        permissionLookup.PermissionCode.Should().Be("A.B");
        permissionLookup.Module.Should().Be("M");
        permissionLookup.Resource.Should().Be("R");
        permissionLookup.Action.Should().Be("X");
        permissionLookup.Name.Should().Be("N");
        permissionLookup.DisplayOrder.Should().Be(1);
        var permissionFilters = new FgsPermissionListFilters("A.B", "M", "R", "X");
        permissionFilters.PermissionCode.Should().Be("A.B");
        permissionFilters.Module.Should().Be("M");
        permissionFilters.Resource.Should().Be("R");
        permissionFilters.Action.Should().Be("X");

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
        serviceSetup.TimeCardOptionId.Should().Be(TimeCardOption.None);
        serviceSetup.AccountingIntegrationTypeId.Should().BeNull();
        serviceSetup.UseExternalTaxCalculationProvider.Should().BeFalse();
        serviceSetup.EnableCallBookingWidget.Should().BeFalse();
        serviceSetup.EnablePaymentWidget.Should().BeFalse();
        serviceSetup.EnableRulesManagement.Should().BeFalse();
        serviceSetup.EnableAutoArrive.Should().BeFalse();
        serviceSetup.WorkLocationRadiusForAutoArrive.Should().BeNull();
        serviceSetup.OTStartTime.Should().BeNull();
        serviceSetup.OTEndTime.Should().BeNull();
        serviceSetup.DTStartTime.Should().BeNull();
        serviceSetup.DTEndTime.Should().BeNull();
        serviceSetup.BillHoursFromDispatchOrArrive.Should().Be("ARRIVE");
        serviceSetup.SourceCodeRequiredOnWorkOrder.Should().BeFalse();
        serviceSetup.SourceCodeRequiredOnServiceLocation.Should().BeFalse();
        serviceSetup.BillToStartNumber.Should().Be(1);
        serviceSetup.POStartNumber.Should().Be(1);
        serviceSetup.QuoteStartNumber.Should().Be(1);
        serviceSetup.WorkOrderStartNumber.Should().Be(1);
        serviceSetup.InvoiceNumberPrefix.Should().BeNull();
        serviceSetup.QuoteNumberPrefix.Should().BeNull();
        serviceSetup.PONumberPrefix.Should().BeNull();
        serviceSetup.WorkOrderNumberPrefix.Should().BeNull();
        serviceSetup.InvoiceBatchNumberFormat.Should().BeNull();
        serviceSetup.EstimateRevisionCreationMode.Should().Be(EstimateRevisionCreationModes.OnDemand);
        serviceSetup.IsActive.Should().BeTrue();
        (serviceSetup with { EnableCustomerPortal = true }).TenantId.Should().Be(1);

        var serviceAccounts = new FgsTenantServiceAccountsSetupDetailDto(1, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, true);
        serviceAccounts.TenantId.Should().Be(1);
        serviceAccounts.CompanyId.Should().Be(1);
        serviceAccounts.BankAccountId.Should().Be(1);
        serviceAccounts.AccountsReceivableAccountId.Should().Be(2);
        serviceAccounts.RevenueAccountId.Should().Be(3);
        serviceAccounts.DiscountAccountId.Should().Be(4);
        serviceAccounts.SalesTaxPayableAccountId.Should().Be(5);
        serviceAccounts.InventoryAccountId.Should().Be(6);
        serviceAccounts.COGSAccountId.Should().Be(7);
        serviceAccounts.UndepositedFundsAccountId.Should().Be(8);
        serviceAccounts.ProcessingFeeAccountId.Should().Be(9);
        serviceAccounts.AccountsPayableAccountId.Should().Be(10);
        (serviceAccounts with { IsActive = false }).BankAccountId.Should().Be(1);

        var rolePermission = new FgsRolePermissionDetailDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        rolePermission.Id.Should().Be(1);
        rolePermission.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        rolePermission.CreatedBy.Should().Be("t");
        (rolePermission with { FgsPermissionId = 3 }).FgsRoleId.Should().Be(1);
        var rolePermissionLookup = new FgsRolePermissionLookupDto(1, 1, 2);
        rolePermissionLookup.Id.Should().Be(1);
        rolePermissionLookup.FgsRoleId.Should().Be(1);
        rolePermissionLookup.FgsPermissionId.Should().Be(2);
        var rolePermissionCreate = new FgsRolePermissionCreateDto(1, 2);
        rolePermissionCreate.FgsPermissionId.Should().Be(2);
        (rolePermissionCreate with { FgsPermissionId = 3 }).FgsRoleId.Should().Be(1);
        var rolePermissionUpdate = new FgsRolePermissionUpdateDto(5);
        rolePermissionUpdate.FgsPermissionId.Should().Be(5);
        var rolePermissionPatch = new FgsRolePermissionPatchDto(FgsPermissionId: 6);
        rolePermissionPatch.FgsPermissionId.Should().Be(6);

        var roleMenu = new FgsRoleMenuDetailDto(1, 10, 100, 1, true, DateTimeOffset.UtcNow, "t");
        roleMenu.Id.Should().Be(1);
        roleMenu.RoleId.Should().Be(10);
        roleMenu.MenuId.Should().Be(100);
        roleMenu.DisplayOrder.Should().Be(1);
        roleMenu.IsActive.Should().BeTrue();
        roleMenu.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        roleMenu.CreatedBy.Should().Be("t");
        (roleMenu with { DisplayOrder = 2 }).DisplayOrder.Should().Be(2);
        var roleMenuSyncItem = new FgsRoleMenuSyncItemDto(100, 1, true);
        roleMenuSyncItem.MenuId.Should().Be(100);
        roleMenuSyncItem.DisplayOrder.Should().Be(1);
        roleMenuSyncItem.IsActive.Should().BeTrue();
        var roleMenuSync = new FgsRoleMenuSyncDto(10, [roleMenuSyncItem]);
        (roleMenuSync with { RoleId = 11 }).Items.Should().ContainSingle(x => x.MenuId == 100);
        var roleMenuLookup = new FgsRoleMenuLookupDto(1, 10, 100, 1);
        roleMenuLookup.Id.Should().Be(1);
        roleMenuLookup.RoleId.Should().Be(10);
        roleMenuLookup.MenuId.Should().Be(100);
        roleMenuLookup.DisplayOrder.Should().Be(1);
        var roleMenuCreate = new FgsRoleMenuCreateDto(10, 100, 2);
        roleMenuCreate.RoleId.Should().Be(10);
        roleMenuCreate.MenuId.Should().Be(100);
        roleMenuCreate.DisplayOrder.Should().Be(2);
        var roleMenuUpdate = new FgsRoleMenuUpdateDto(10, 101, 3);
        roleMenuUpdate.RoleId.Should().Be(10);
        roleMenuUpdate.MenuId.Should().Be(101);
        roleMenuUpdate.DisplayOrder.Should().Be(3);
        var roleMenuPatch = new FgsRoleMenuPatchDto(RoleId: 11, MenuId: 200, IsActive: false, DisplayOrder: 3);
        roleMenuPatch.RoleId.Should().Be(11);
        roleMenuPatch.MenuId.Should().Be(200);
        roleMenuPatch.IsActive.Should().BeFalse();
        roleMenuPatch.DisplayOrder.Should().Be((short)3);

        var tenantMenu = new FgsTenantMenuDetailDto(
            1, 100, "DASHBOARD", "Dashboard", "Home", 5, "PAGE", "/d", "home", 1, true, DateTimeOffset.UtcNow, "t");
        tenantMenu.Id.Should().Be(1);
        tenantMenu.MenuId.Should().Be(100);
        tenantMenu.MenuCode.Should().Be("DASHBOARD");
        tenantMenu.Name.Should().Be("Dashboard");
        tenantMenu.Description.Should().Be("Home");
        tenantMenu.ParentMenuId.Should().Be(5);
        tenantMenu.MenuType.Should().Be("PAGE");
        tenantMenu.Route.Should().Be("/d");
        tenantMenu.Icon.Should().Be("home");
        tenantMenu.DisplayOrder.Should().Be(1);
        tenantMenu.IsActive.Should().BeTrue();
        tenantMenu.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        tenantMenu.CreatedBy.Should().Be("t");
        (tenantMenu with { IsActive = false }).IsActive.Should().BeFalse();
        var tenantMenuSync = new FgsTenantMenuSyncDto(
            [new FgsTenantMenuSyncItemDto(100, "DASHBOARD", "Dashboard", "PAGE")]);
        (tenantMenuSync with { Items = [new FgsTenantMenuSyncItemDto(101, "SETTINGS", "Settings", "GROUP", DisplayOrder: 2)] }).Items
            .Should().ContainSingle(x => x.MenuId == 101 && x.MenuCode == "SETTINGS");
        var syncItem = new FgsTenantMenuSyncItemDto(
            200,
            "CHILD",
            "Child",
            "PAGE",
            Description: "Nested",
            ParentMenuId: 100,
            Route: "/child",
            Icon: "folder",
            DisplayOrder: 3,
            IsActive: false);
        syncItem.Description.Should().Be("Nested");
        syncItem.ParentMenuId.Should().Be(100);
        syncItem.Route.Should().Be("/child");
        syncItem.Icon.Should().Be("folder");
        syncItem.DisplayOrder.Should().Be(3);
        syncItem.IsActive.Should().BeFalse();
        var lookup = new FgsTenantMenuLookupDto(1, 100, "DASHBOARD", "Dashboard", 1);
        lookup.Id.Should().Be(1);
        lookup.MenuId.Should().Be(100);
        lookup.MenuCode.Should().Be("DASHBOARD");
        lookup.Name.Should().Be("Dashboard");
        lookup.DisplayOrder.Should().Be(1);
        var create = new FgsTenantMenuCreateDto(100, "DASHBOARD", "Dashboard", "PAGE", Description: "d", ParentMenuId: 1, Route: "/d", Icon: "home", DisplayOrder: 2);
        create.MenuId.Should().Be(100);
        create.MenuCode.Should().Be("DASHBOARD");
        create.Name.Should().Be("Dashboard");
        create.MenuType.Should().Be("PAGE");
        create.Description.Should().Be("d");
        create.ParentMenuId.Should().Be(1);
        create.Route.Should().Be("/d");
        create.Icon.Should().Be("home");
        create.DisplayOrder.Should().Be(2);
        var update = new FgsTenantMenuUpdateDto(100, "DASHBOARD", "Home", "PAGE", Description: "u", ParentMenuId: 2, Route: "/h", Icon: "house", DisplayOrder: 3);
        update.MenuId.Should().Be(100);
        update.MenuCode.Should().Be("DASHBOARD");
        update.Name.Should().Be("Home");
        update.MenuType.Should().Be("PAGE");
        update.Description.Should().Be("u");
        update.ParentMenuId.Should().Be(2);
        update.Route.Should().Be("/h");
        update.Icon.Should().Be("house");
        update.DisplayOrder.Should().Be(3);
        var patch = new FgsTenantMenuPatchDto(
            MenuId: 101,
            MenuCode: "SETTINGS",
            Name: "Settings",
            Description: "s",
            ParentMenuId: 3,
            MenuType: "GROUP",
            Route: "/s",
            Icon: "gear",
            IsActive: false,
            DisplayOrder: 2);
        patch.MenuId.Should().Be(101);
        patch.MenuCode.Should().Be("SETTINGS");
        patch.Name.Should().Be("Settings");
        patch.Description.Should().Be("s");
        patch.ParentMenuId.Should().Be(3);
        patch.MenuType.Should().Be("GROUP");
        patch.Route.Should().Be("/s");
        patch.Icon.Should().Be("gear");
        patch.IsActive.Should().BeFalse();
        patch.DisplayOrder.Should().Be((short)2);

        var rolePermissionSummary = new FgsRolePermissionSummaryDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        (rolePermissionSummary with { FgsPermissionId = 3 }).FgsRoleId.Should().Be(1);
        rolePermissionSummary.Id.Should().Be(1);
        rolePermissionSummary.FgsPermissionId.Should().Be(2);
        rolePermissionSummary.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        rolePermissionSummary.CreatedBy.Should().Be("t");

        var roleDataAccessSummary = new FgsRoleDataAccessSummaryDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        (roleDataAccessSummary with { FgsDataAccessId = 3 }).FgsRoleId.Should().Be(1);
        roleDataAccessSummary.Id.Should().Be(1);
        roleDataAccessSummary.FgsDataAccessId.Should().Be(2);
        roleDataAccessSummary.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        roleDataAccessSummary.CreatedBy.Should().Be("t");

        var userRoleSummary = new FgsUserRoleSummaryDto(1, Guid.NewGuid(), 2, DateTimeOffset.UtcNow, "t");
        (userRoleSummary with { FgsRoleId = 3 }).CreatedBy.Should().Be("t");
        userRoleSummary.Id.Should().Be(1);
        userRoleSummary.UserId.Should().NotBe(Guid.Empty);
        userRoleSummary.FgsRoleId.Should().Be(2);
        userRoleSummary.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));

        var roleDataAccess = new FgsRoleDataAccessDetailDto(1, 1, 2, DateTimeOffset.UtcNow, "t");
        roleDataAccess.Id.Should().Be(1);
        roleDataAccess.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        roleDataAccess.CreatedBy.Should().Be("t");
        (roleDataAccess with { FgsDataAccessId = 3 }).FgsRoleId.Should().Be(1);
        var roleDataAccessLookup = new FgsRoleDataAccessLookupDto(1, 1, 2);
        roleDataAccessLookup.Id.Should().Be(1);
        roleDataAccessLookup.FgsRoleId.Should().Be(1);
        roleDataAccessLookup.FgsDataAccessId.Should().Be(2);
        var roleDataAccessCreate = new FgsRoleDataAccessCreateDto(1, 2);
        roleDataAccessCreate.FgsDataAccessId.Should().Be(2);
        (roleDataAccessCreate with { FgsDataAccessId = 3 }).FgsRoleId.Should().Be(1);
        var roleDataAccessUpdate = new FgsRoleDataAccessUpdateDto(5);
        roleDataAccessUpdate.FgsDataAccessId.Should().Be(5);
        var roleDataAccessPatch = new FgsRoleDataAccessPatchDto(FgsDataAccessId: 6);
        roleDataAccessPatch.FgsDataAccessId.Should().Be(6);

        var userRole = new FgsUserRoleDetailDto(1, Guid.NewGuid(), 2, DateTimeOffset.UtcNow, "t");
        userRole.Id.Should().Be(1);
        userRole.UserId.Should().NotBe(Guid.Empty);
        userRole.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        (userRole with { FgsRoleId = 3 }).CreatedBy.Should().Be("t");
        var userRoleLookup = new FgsUserRoleLookupDto(1, Guid.NewGuid(), 2);
        userRoleLookup.Id.Should().Be(1);
        userRoleLookup.UserId.Should().NotBe(Guid.Empty);
        userRoleLookup.FgsRoleId.Should().Be(2);
        var userRoleCreate = new FgsUserRoleCreateDto(Guid.NewGuid(), 2);
        userRoleCreate.FgsRoleId.Should().Be(2);
        var userRoleUpdate = new FgsUserRoleUpdateDto(3);
        userRoleUpdate.FgsRoleId.Should().Be(3);
        var userRolePatch = new FgsUserRolePatchDto(FgsRoleId: 4);
        userRolePatch.FgsRoleId.Should().Be(4);

        var location = new LocationDetailDto(Guid.NewGuid(), "1", null, null, null, "Austin", "TX", null, "US", "78701", null, null, null, null, true);
        (location with { City = "Dallas" }).State.Should().Be("TX");

        var filters = new FgsApiClientListFilters("a", "e");
        filters.ApplicationName.Should().Be("a");

        var patchSetup = new FgsTenantServiceSetupPatchDto(EnableCustomerPortal: true, BillToStartNumber: 200);
        patchSetup.EnableCustomerPortal.Should().BeTrue();
    }
}
