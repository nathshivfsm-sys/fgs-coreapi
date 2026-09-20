using System;
using System.Threading;
using System.Threading.Tasks;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Time;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.DeleteFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.PatchFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

var tenantId = 10L; var companyId = 20L;
var accessor = new TestTenant { Current = new TenantContext { TenantId = tenantId, CompanyId = companyId } };
var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString())
    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    .Options;
await using var context = new FgsSetupDbContext(options, accessor);
await context.Database.EnsureCreatedAsync();
await context.GloMasterEntityTypes.AddAsync(new GloMasterEntityType { Id = 15, Code = ""EMPLOYEE"", IsDocumentAllowed = true, IsActive = true, SortOrder = 15, CreatedOn = DateTimeOffset.UtcNow, CreatedBy = ""t"" });
await context.SaveChangesAsync();

var userContext = new Mock<IFgsUserContext>();
userContext.SetupGet(x => x.TenantId).Returns(tenantId);
userContext.SetupGet(x => x.CompanyId).Returns(companyId);
userContext.SetupGet(x => x.UserId).Returns(Guid.Parse(""11111111-1111-1111-1111-111111111111""));
var audit = new SetupEntityAuditHelper(userContext.Object, accessor, new DateTimeProvider());
var uow = new EfUnitOfWork<FgsSetupDbContext>(context);
var locations = new SetupLocationWriteService(context, uow, audit);
var write = new FgsEmployeeWriteService(context, uow, audit, locations);
var cache = new Mock<ICacheService>();
var create = new CreateFgsEmployeeCommandHandler(write, cache.Object, accessor, NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
var delete = new DeleteFgsEmployeeCommandHandler(write, cache.Object, accessor, NullLogger<DeleteFgsEmployeeCommandHandler>.Instance);
var patch = new PatchFgsEmployeeCommandHandler(write, cache.Object, accessor, NullLogger<PatchFgsEmployeeCommandHandler>.Instance);

var created = await create.Handle(new CreateFgsEmployeeCommand(new FgsEmployeeCreateDto(null, ""EMP-1"", EmployeeTypeIds.Office, ""A"", ""A"", null, ""B"", null, DateOnly.FromDateTime(DateTime.UtcNow), null, EmployeeStatusIds.Active, null, ""a@b.com"", null, ""1"", new Fgs.Setup.Application.Common.Locations.LocationWriteDto(""1 Main"", null, null, null, ""Austin"", ""TX"", null, ""US"", ""78701"", null, null, null, null), null, 40m, LaborBurdenTypeIds.Percentage, 10m)), default);
Console.WriteLine($""Created Status={created.Data!.StatusId} Addr={created.Data.Address?.AddressLine1}"");
var del = await delete.Handle(new DeleteFgsEmployeeCommand(created.Data.Id), default);
Console.WriteLine($""Deleted Status={del.Data!.StatusId} AddrNull={del.Data.Address is null} LocActive={context.FgsLocations.Single().IsActive}"");
var patched = await patch.Handle(new PatchFgsEmployeeCommand(created.Data.Id, new FgsEmployeePatchDto(null,null,null,null,null,null,null,null,null,null, EmployeeStatusIds.Active, null,null,null,null,null,null,null,null,null,null,null,null,null)), default);
Console.WriteLine($""Patched Success={patched.Success} Status={patched.Data?.StatusId} AddrNull={patched.Data?.Address is null} LocActive={context.FgsLocations.Single().IsActive}"");

sealed class TestTenant : ITenantContextAccessor { public ITenantContext? Current { get; set; } }
