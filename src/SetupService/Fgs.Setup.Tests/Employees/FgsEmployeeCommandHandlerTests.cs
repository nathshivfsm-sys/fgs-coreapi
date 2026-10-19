using FluentValidation;
using Fgs.Contracts.Api;
using Fgs.Contracts.Audit;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.DeleteFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.PatchFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.UpdateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Entities.Locations;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Entities.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.Employees;

public sealed class FgsEmployeeCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesEmployeeWithAddressAndCalculatedRates()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsEmployeeCommand(CreateDto(regularRate: 40m)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.StatusId.Should().Be(EmployeeStatusIds.Active);
        response.Data.OvertimeRate.Should().Be(60m);
        response.Data.DoubleTimeRate.Should().Be(80m);
        response.Data.Address.Should().NotBeNull();
        response.Data.Address!.AddressLine1.Should().Be("100 Main St");
        response.Data.Address.PostalCode.Should().Be("78701");
        response.Data.TechnicianProfile.Should().NotBeNull();
        response.Data.TechnicianProfile!.TechCode.Should().Be("T-001");
        response.Data.TechnicianProfile.CanBeScheduled.Should().BeTrue();
        response.Data.TechnicianProfile.StartLocationTypeId.Should().Be(StartLocationTypeIds.Office);
        context.FgsLocations.Should().ContainSingle(l => l.IsActive && l.AddressLine1 == "100 Main St");
        context.FgsEmployeeTechnicianProfiles.Should().ContainSingle(p => p.TechCode == "T-001");
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "employees"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateHandler_WhenOfficeWithoutProfile_DoesNotCreateTechnicianProfile()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsEmployeeCommand(CreateOfficeDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TechnicianProfile.Should().BeNull();
        context.FgsEmployeeTechnicianProfiles.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateHandler_WhenCanBeScheduledAndIsPurchaserOmitted_PersistsBothFalse()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var handler = new CreateFgsEmployeeCommandHandler(
            CreateWriteService(context),
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var profile = new FgsEmployeeTechnicianProfileWriteDto(
            TechCode: "T-200",
            TechName: "Alex",
            StartLocationTypeId: StartLocationTypeIds.Office);
        var dto = new FgsEmployeeCreateDto(
            UserId: null,
            EmployeeNumber: "EMP-200",
            EmployeeTypeId: EmployeeTypeIds.Technician,
            DisplayName: "Alex Tech",
            LegalFirstName: "Alex",
            LegalMiddleName: null,
            LegalLastName: "Tech",
            BirthDate: null,
            HireDate: new DateOnly(2026, 1, 15),
            TerminationDate: null,
            StatusId: EmployeeStatusIds.Active,
            PersonalEmail: null,
            OfficeEmail: "alex.defaults@example.com",
            PersonalPhone: null,
            OfficePhone: "+15551234567",
            Address: CreateAddress(),
            ProfilePhotoFileId: null,
            RegularRate: 40m,
            LaborBurdenTypeId: null,
            LaborBurdenValue: null,
            TechnicianProfile: profile);

        var response = await handler.Handle(new CreateFgsEmployeeCommand(dto), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsPurchaser.Should().BeFalse();
        response.Data.TechnicianProfile!.CanBeScheduled.Should().BeFalse();
        context.FgsEmployees.Single().IsPurchaser.Should().BeFalse();
        context.FgsEmployeeTechnicianProfiles.Single().CanBeScheduled.Should().BeFalse();
    }

    [Fact]
    public async Task CreateHandler_WhenLegalNamesNull_Succeeds()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var handler = new CreateFgsEmployeeCommandHandler(
            CreateWriteService(context),
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsEmployeeCommand(CreateDto() with
            {
                LegalFirstName = null,
                LegalMiddleName = "   ",
                LegalLastName = null
            }),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.LegalFirstName.Should().BeNull();
        response.Data.LegalMiddleName.Should().BeNull();
        response.Data.LegalLastName.Should().BeNull();
        var stored = context.FgsEmployees.Single();
        stored.LegalFirstName.Should().BeNull();
        stored.LegalMiddleName.Should().BeNull();
        stored.LegalLastName.Should().BeNull();
    }

    [Fact]
    public async Task CreateHandler_WhenTechCodeBlank_StoresTrimmedEmployeeNumber()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var handler = new CreateFgsEmployeeCommandHandler(
            CreateWriteService(context),
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsEmployeeCommand(CreateDto() with
            {
                EmployeeNumber = "  EMP-100  ",
                OfficeEmail = "tech.default@example.com",
                TechnicianProfile = CreateTechnicianProfile() with { TechCode = "   " }
            }),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.EmployeeNumber.Should().Be("EMP-000001");
        response.Data.TechnicianProfile!.TechCode.Should().Be("EMP-000001");
        context.FgsEmployeeTechnicianProfiles.Single().TechCode.Should().Be("EMP-000001");
    }

    [Fact]
    public async Task CreateHandler_WhenCodesHaveOnlyLeadingOrTrailingSpaces_StoresTrimmedValues()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var handler = new CreateFgsEmployeeCommandHandler(
            CreateWriteService(context),
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsEmployeeCommand(CreateDto() with
            {
                EmployeeNumber = "  EMP-001  ",
                TechnicianProfile = CreateTechnicianProfile() with { TechCode = "  T-001  " }
            }),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.EmployeeNumber.Should().Be("EMP-000001");
        response.Data.TechnicianProfile!.TechCode.Should().Be("T-001");
    }

    [Fact]
    public async Task UpdateHandler_WhenSwitchingToOffice_RemovesTechnicianProfile()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var updateHandler = new UpdateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsEmployeeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto()),
            CancellationToken.None);
        created.Success.Should().BeTrue();
        context.FgsEmployeeTechnicianProfiles.Should().ContainSingle();

        var response = await updateHandler.Handle(
            new UpdateFgsEmployeeCommand(created.Data!.Id, CreateOfficeUpdateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TechnicianProfile.Should().BeNull();
        context.FgsEmployeeTechnicianProfiles.Should().BeEmpty();
    }

    [Fact]
    public async Task PatchHandler_WhenInactive_ReactivatesStatusAndAddress()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsEmployeeCommandHandler>.Instance);
        var patchHandler = new PatchFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<PatchFgsEmployeeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto()),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var deleted = await deleteHandler.Handle(
            new DeleteFgsEmployeeCommand(created.Data!.Id),
            CancellationToken.None);
        deleted.Data!.StatusId.Should().Be(EmployeeStatusIds.Inactive);
        deleted.Data.Address.Should().BeNull();
        context.FgsLocations.Single().IsActive.Should().BeFalse();

        var response = await patchHandler.Handle(
            new PatchFgsEmployeeCommand(
                created.Data.Id,
                CreateStatusPatch(statusId: EmployeeStatusIds.Active)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.StatusId.Should().Be(EmployeeStatusIds.Active);
        response.Data.Address.Should().NotBeNull();
        response.Data.Address!.AddressLine1.Should().Be("100 Main St");
        context.FgsLocations.Single().IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task PatchHandler_WhenInactive_IsActiveTrueReactivates()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var patchHandler = new PatchFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<PatchFgsEmployeeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto()),
            CancellationToken.None);

        await patchHandler.Handle(
            new PatchFgsEmployeeCommand(
                created.Data!.Id,
                CreateStatusPatch(statusId: EmployeeStatusIds.Inactive)),
            CancellationToken.None);

        var response = await patchHandler.Handle(
            new PatchFgsEmployeeCommand(
                created.Data.Id,
                CreateStatusPatch(isActive: true)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.StatusId.Should().Be(EmployeeStatusIds.Active);
        response.Data.Address.Should().NotBeNull();
        context.FgsLocations.Single().IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task PatchHandler_WhenInactive_CanPatchOtherFields()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var patchHandler = new PatchFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<PatchFgsEmployeeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto()),
            CancellationToken.None);

        await patchHandler.Handle(
            new PatchFgsEmployeeCommand(
                created.Data!.Id,
                CreateStatusPatch(statusId: EmployeeStatusIds.Inactive)),
            CancellationToken.None);

        var response = await patchHandler.Handle(
            new PatchFgsEmployeeCommand(
                created.Data.Id,
                CreateStatusPatch(displayName: "Alex Updated")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.StatusId.Should().Be(EmployeeStatusIds.Inactive);
        response.Data.DisplayName.Should().Be("Alex Updated");
    }

    [Fact]
    public async Task DeleteHandler_SetsStatusInactiveAndSoftDeletesAddress()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsEmployeeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto()),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsEmployeeCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.StatusId.Should().Be(EmployeeStatusIds.Inactive);
        context.FgsLocations.Single().IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task PatchHandler_WhenStatusChangesWithUserId_EnqueuesAccessEventAndStatusAudit()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var userClient = new Mock<IUserInternalUsersClient>();
        var auditRecorder = new Mock<IEmployeeAuditRecorder>();
        var writeService = CreateWriteService(context, userClient, auditRecorder);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var patchHandler = new PatchFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<PatchFgsEmployeeCommandHandler>.Instance);

        var linkedUserId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto() with { UserId = linkedUserId }),
            CancellationToken.None);

        await patchHandler.Handle(
            new PatchFgsEmployeeCommand(
                created.Data!.Id,
                CreateStatusPatch(statusId: EmployeeStatusIds.Inactive)),
            CancellationToken.None);

        userClient.Verify(
            c => c.SetUserAccessAsync(
                linkedUserId,
                It.Is<SetUserAccessRequest>(r => r.IsActive == false),
                TenantId.ToString(),
                CompanyId.ToString(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        auditRecorder.Verify(
            r => r.RecordAsync(
                It.Is<RecordAuditEventRequest>(req =>
                    req.EventCode == "EMPLOYEE_STATUS_CHANGED"
                    && req.Summary == "Active → Inactive"
                    && req.Details!.Any(d =>
                        d.ItemName == "Status"
                        && d.OldValue == "Active"
                        && d.NewValue == "Inactive")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PatchHandler_WhenStatusChangesWithoutUserId_DoesNotEnqueueAccessEvent()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var userClient = new Mock<IUserInternalUsersClient>();
        var writeService = CreateWriteService(context, userClient);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var patchHandler = new PatchFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<PatchFgsEmployeeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto()),
            CancellationToken.None);

        await patchHandler.Handle(
            new PatchFgsEmployeeCommand(
                created.Data!.Id,
                CreateStatusPatch(statusId: EmployeeStatusIds.Inactive)),
            CancellationToken.None);

        userClient.Verify(
            c => c.SetUserAccessAsync(
                It.IsAny<Guid>(),
                It.IsAny<SetUserAccessRequest>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateHandler_WhenEmployeeNumberMissingOrArbitrary_AssignsSequencePerCompany()
    {
        var databaseName = Guid.NewGuid().ToString();
        await using var context = await CreateContextAsync(databaseName: databaseName);
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);

        var missing = await writeService.CreateAsync(
            CreateOfficeDto() with { EmployeeNumber = null },
            CancellationToken.None);
        var arbitrary = await writeService.CreateAsync(
            CreateOfficeDto() with
            {
                EmployeeNumber = "EMP-200",
                OfficeEmail = "second.office@example.com"
            },
            CancellationToken.None);

        missing.EmployeeNumber.Should().Be("EMP-000001");
        arbitrary.EmployeeNumber.Should().Be("EMP-000002");

        await using var otherCompany = await CreateContextAsync(companyId: CompanyId + 1, databaseName: databaseName);
        var otherWriteService = CreateWriteService(otherCompany, CompanyId + 1);
        var restarted = await otherWriteService.CreateAsync(
            CreateOfficeDto() with { OfficeEmail = "other.company@example.com" },
            CancellationToken.None);

        restarted.EmployeeNumber.Should().Be("EMP-000001");
    }

    [Fact]
    public async Task CreateHandler_WhenMatchingNumberExists_AssignsNextAndIgnoresNonMatching()
    {
        await using var terminatedContext = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(terminatedContext);
        await SeedEmployeeAsync(terminatedContext, "EMP-000005", EmployeeStatusIds.Terminated);
        await SeedEmployeeAsync(terminatedContext, "EMP-200", EmployeeStatusIds.Active);
        await SeedEmployeeAsync(terminatedContext, "EMP-000009", EmployeeStatusIds.Active, companyId: CompanyId + 1);

        var nextAfterTerminated = await CreateWriteService(terminatedContext).CreateAsync(
            CreateOfficeDto(),
            CancellationToken.None);

        nextAfterTerminated.EmployeeNumber.Should().Be("EMP-000006");

        await using var inactiveContext = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(inactiveContext);
        await SeedEmployeeAsync(inactiveContext, "EMP-000005", EmployeeStatusIds.Inactive);

        var nextAfterInactive = await CreateWriteService(inactiveContext).CreateAsync(
            CreateOfficeDto(),
            CancellationToken.None);

        nextAfterInactive.EmployeeNumber.Should().Be("EMP-000006");
    }

    [Fact]
    public async Task CreateHandler_WhenGeneratedTechCodeAlreadyExists_ThrowsValidationException()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var existing = new FgsEmployee
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            EmployeeNumber = "EMP-200",
            EmployeeTypeId = EmployeeTypeIds.Technician,
            DisplayName = "Existing Tech",
            StatusId = EmployeeStatusIds.Active,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = 1
        };
        context.FgsEmployees.Add(existing);
        await context.SaveChangesAsync();
        context.FgsEmployeeTechnicianProfiles.Add(new FgsEmployeeTechnicianProfile
        {
            EmployeeId = existing.Id,
            TenantId = TenantId,
            CompanyId = CompanyId,
            TechCode = "EMP-000001",
            DailyCapacityHours = 8m,
            StartLocationTypeId = StartLocationTypeIds.Office,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = 1
        });
        await context.SaveChangesAsync();

        var act = async () => await CreateWriteService(context).CreateAsync(
            CreateDto() with
            {
                EmployeeNumber = null,
                OfficeEmail = "new.tech@example.com",
                TechnicianProfile = CreateTechnicianProfile() with { TechCode = "   " }
            },
            CancellationToken.None);

        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().Contain(e =>
            e.ErrorMessage == "A technician with this tech code already exists.");
        context.FgsEmployeeTechnicianProfiles.Should().ContainSingle(p => p.TechCode == "EMP-000001");
    }

    [Fact]
    public async Task UpdateAndPatch_StoreRequestedEmployeeNumber()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var created = await writeService.CreateAsync(CreateOfficeDto(), CancellationToken.None);
        created.EmployeeNumber.Should().Be("EMP-000001");

        var updated = await writeService.UpdateAsync(
            created.Id,
            CreateOfficeUpdateDto() with { EmployeeNumber = "  EMP-KEEP  " },
            CancellationToken.None);
        var patched = await writeService.PatchAsync(
            created.Id,
            CreateStatusPatch(employeeNumber: "  EMP-PATCH  "),
            CancellationToken.None);

        updated.EmployeeNumber.Should().Be("EMP-KEEP");
        patched.EmployeeNumber.Should().Be("EMP-PATCH");
    }

    private static FgsEmployeePatchDto CreateStatusPatch(
        short? statusId = null,
        bool? isActive = null,
        string? displayName = null,
        string? employeeNumber = null) =>
        new(
            UserId: null,
            EmployeeNumber: employeeNumber,
            EmployeeTypeId: null,
            DisplayName: displayName,
            LegalFirstName: null,
            LegalMiddleName: null,
            LegalLastName: null,
            BirthDate: null,
            HireDate: null,
            TerminationDate: null,
            StatusId: statusId,
            PersonalEmail: null,
            OfficeEmail: null,
            PersonalPhone: null,
            OfficePhone: null,
            Address: null,
            ProfilePhotoFileId: null,
            RegularRate: null,
            OvertimeRate: null,
            DoubleTimeRate: null,
            LaborBurdenTypeId: null,
            LaborBurdenValue: null,
            IsPurchaser: null,
            Notes: null,
            IsActive: isActive,
            TechnicianProfile: null);

    private static FgsEmployeeCreateDto CreateDto(decimal? regularRate = null) =>
        new(
            UserId: null,
            EmployeeNumber: "EMP-001",
            EmployeeTypeId: EmployeeTypeIds.Technician,
            DisplayName: "Alex Tech",
            LegalFirstName: "Alex",
            LegalMiddleName: null,
            LegalLastName: "Tech",
            BirthDate: null,
            HireDate: new DateOnly(2026, 1, 15),
            TerminationDate: null,
            StatusId: EmployeeStatusIds.Active,
            PersonalEmail: "alex.personal@example.com",
            OfficeEmail: "alex@example.com",
            PersonalPhone: null,
            OfficePhone: "+15551234567",
            Address: CreateAddress(),
            ProfilePhotoFileId: null,
            RegularRate: regularRate,
            LaborBurdenTypeId: LaborBurdenTypeIds.Percentage,
            LaborBurdenValue: 25m,
            IsPurchaser: false,
            Notes: "Field tech",
            TechnicianProfile: CreateTechnicianProfile());

    private static FgsEmployeeCreateDto CreateOfficeDto() =>
        new(
            UserId: null,
            EmployeeNumber: "EMP-OFF-001",
            EmployeeTypeId: EmployeeTypeIds.Office,
            DisplayName: "Alex Office",
            LegalFirstName: "Alex",
            LegalMiddleName: null,
            LegalLastName: "Office",
            BirthDate: null,
            HireDate: new DateOnly(2026, 1, 15),
            TerminationDate: null,
            StatusId: EmployeeStatusIds.Active,
            PersonalEmail: null,
            OfficeEmail: "alex.office@example.com",
            PersonalPhone: null,
            OfficePhone: "+15551234567",
            Address: CreateAddress(),
            ProfilePhotoFileId: null,
            RegularRate: 40m,
            LaborBurdenTypeId: LaborBurdenTypeIds.Percentage,
            LaborBurdenValue: 25m,
            IsPurchaser: false,
            Notes: null,
            TechnicianProfile: null);

    private static FgsEmployeeUpdateDto CreateOfficeUpdateDto() =>
        new(
            UserId: null,
            EmployeeNumber: "EMP-001",
            EmployeeTypeId: EmployeeTypeIds.Office,
            DisplayName: "Alex Tech",
            LegalFirstName: "Alex",
            LegalMiddleName: null,
            LegalLastName: "Tech",
            BirthDate: null,
            HireDate: new DateOnly(2026, 1, 15),
            TerminationDate: null,
            StatusId: EmployeeStatusIds.Active,
            PersonalEmail: "alex.personal@example.com",
            OfficeEmail: "alex@example.com",
            PersonalPhone: null,
            OfficePhone: "+15551234567",
            Address: CreateAddress(),
            ProfilePhotoFileId: null,
            RegularRate: 40m,
            OvertimeRate: 60m,
            DoubleTimeRate: 80m,
            LaborBurdenTypeId: LaborBurdenTypeIds.Percentage,
            LaborBurdenValue: 25m,
            IsPurchaser: false,
            Notes: "Field tech",
            TechnicianProfile: null);

    private static FgsEmployeeTechnicianProfileWriteDto CreateTechnicianProfile() =>
        new(
            TechCode: "T-001",
            TechName: "Alex",
            CanBeScheduled: true,
            DailyCapacityHours: 8m,
            DispatchZoneId: null,
            StartLocationTypeId: StartLocationTypeIds.Office,
            StartTime: new TimeOnly(8, 0),
            TechTradeId: 1,
            TechSkillId: 2,
            TruckId: null,
            CustomerFacingPhone: "+15559876543",
            Notes: "Mobile bio");

    [Fact]
    public async Task Patch_DoesNotAttachAnotherCompanysAddress()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var created = await writeService.CreateAsync(CreateDto(regularRate: 40m), CancellationToken.None);
        var foreignAddressId = Guid.NewGuid();
        context.FgsLocations.Add(new FgsLocation
        {
            Id = foreignAddressId,
            TenantId = 99,
            CompanyId = 99,
            MasterEntityTypeId = 15,
            AddressLine1 = "Other company",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        var employee = await context.FgsEmployees.SingleAsync(e => e.Id == created.Id);
        employee.AddressId = foreignAddressId;
        await context.SaveChangesAsync();

        var patched = await writeService.PatchAsync(
            created.Id,
            new FgsEmployeePatchDto(
                null, null, null, "Renamed", null, null, null, null, null, null, null,
                null, null, null, null, null, null, null, null, null, null, null, null, null),
            CancellationToken.None);

        patched.DisplayName.Should().Be("Renamed");
        patched.Address.Should().BeNull();
    }

    [Fact]
    public async Task LocationSoftDelete_DoesNotUpdateAnotherCompanysLocation()
    {
        await using var context = await CreateContextAsync();
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var locationWriteService = new SetupLocationWriteService(
            context,
            new EfUnitOfWork<FgsSetupDbContext>(context),
            new SetupEntityAuditHelper(userContext.Object, CreateTenantContextAccessor(), new DateTimeProvider()));
        var foreignId = Guid.NewGuid();
        context.FgsLocations.Add(new FgsLocation
        {
            Id = foreignId,
            TenantId = 99,
            CompanyId = 99,
            MasterEntityTypeId = 15,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();

        await locationWriteService.SoftDeleteAsync(foreignId, CancellationToken.None);

        var stored = await context.FgsLocations.IgnoreQueryFilters().SingleAsync(l => l.Id == foreignId);
        stored.IsActive.Should().BeTrue();
    }

    private static LocationWriteDto CreateAddress() =>
        new(
            "100 Main St",
            "Apt 2",
            null,
            null,
            "Austin",
            "TX",
            null,
            "US",
            "78701",
            null,
            null,
            null,
            null);

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsEmployeeWriteService CreateWriteService(
        FgsSetupDbContext context,
        long companyId = CompanyId)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(companyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = companyId }
        };

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        ISetupLocationWriteService locationWriteService = new SetupLocationWriteService(
            context,
            unitOfWork,
            auditHelper);
        var employeeAuditRecorder = new Mock<IEmployeeAuditRecorder>();
        employeeAuditRecorder
            .Setup(r => r.RecordAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return new FgsEmployeeWriteService(
            context,
            unitOfWork,
            auditHelper,
            locationWriteService,
            employeeAuditRecorder.Object,
            CreateUserClient().Object,
            userContext.Object);
    }

    private static FgsEmployeeWriteService CreateWriteService(
        FgsSetupDbContext context,
        Mock<IUserInternalUsersClient> userClient,
        Mock<IEmployeeAuditRecorder>? employeeAuditRecorder = null)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        ISetupLocationWriteService locationWriteService = new SetupLocationWriteService(
            context,
            unitOfWork,
            auditHelper);
        var auditRecorder = employeeAuditRecorder ?? new Mock<IEmployeeAuditRecorder>();
        auditRecorder
            .Setup(r => r.RecordAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return new FgsEmployeeWriteService(
            context,
            unitOfWork,
            auditHelper,
            locationWriteService,
            auditRecorder.Object,
            CreateUserClient(userClient).Object,
            userContext.Object);
    }

    private static Mock<IUserInternalUsersClient> CreateUserClient(
        Mock<IUserInternalUsersClient>? userClient = null)
    {
        var client = userClient ?? new Mock<IUserInternalUsersClient>();
        client
            .Setup(c => c.SetUserAccessAsync(
                It.IsAny<Guid>(),
                It.IsAny<SetUserAccessRequest>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object()));
        return client;
    }

    private static async Task SeedMasterEntityTypeAsync(FgsSetupDbContext context)
    {
        await context.GloMasterEntityTypes.AddAsync(new GloMasterEntityType
        {
            Id = 15,
            Code = "EMPLOYEE",
            IsDocumentAllowed = true,
            IsActive = true,
            SortOrder = 15,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();
    }

    private static async Task SeedEmployeeAsync(
        FgsSetupDbContext context,
        string employeeNumber,
        short statusId,
        long companyId = CompanyId)
    {
        context.FgsEmployees.Add(new FgsEmployee
        {
            TenantId = TenantId,
            CompanyId = companyId,
            EmployeeNumber = employeeNumber,
            EmployeeTypeId = EmployeeTypeIds.Office,
            DisplayName = employeeNumber,
            StatusId = statusId,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = 1
        });
        await context.SaveChangesAsync();
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync(
        long companyId = CompanyId,
        string? databaseName = null)
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = companyId }
        };

        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsSetupDbContext(options, accessor);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
