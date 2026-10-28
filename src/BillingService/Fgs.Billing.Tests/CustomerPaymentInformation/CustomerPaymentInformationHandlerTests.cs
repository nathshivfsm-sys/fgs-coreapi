using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.CreateCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.DeleteCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationById;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.ListCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Validators;
using Fgs.Billing.Domain.Entities;
using Fgs.Billing.Infrastructure.Common;
using Fgs.Billing.Infrastructure.Database;
using Fgs.Billing.Infrastructure.Entities.CustomerPaymentInformation;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Time;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Billing.Tests.CustomerPaymentInformation;

public sealed class CustomerPaymentInformationHandlerTests
{
    [Fact]
    public async Task Create_ReturnsCreatedGridRow()
    {
        var settings = new Mock<ICompanyAuthorizeNetSettings>();
        settings.Setup(s => s.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompanyAuthorizeNetSettingsResult.Found(Merchant()));

        var cim = new Mock<IAuthorizeNetCustomerProfileClient>();
        cim.Setup(c => c.CreateCustomerProfileAsync(It.IsAny<CompanyAuthorizeNetSettings>(), "42", It.IsAny<CancellationToken>()))
            .ReturnsAsync(AuthorizeNetCallResult.Ok("profile-1", null));
        cim.Setup(c => c.CreateCustomerPaymentProfileAsync(
                It.IsAny<CompanyAuthorizeNetSettings>(),
                "profile-1",
                It.IsAny<AuthorizeNetOpaqueData>(),
                "Jane Cardholder",
                "12345",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(AuthorizeNetCallResult.Ok("profile-1", "payment-1"));
        cim.Setup(c => c.GetCustomerPaymentProfileAsync(
                It.IsAny<CompanyAuthorizeNetSettings>(),
                "profile-1",
                "payment-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(AuthorizeNetCallResult.Ok("profile-1", "payment-1", "Visa", "4242", 12, 2028));

        CustomerPaymentInformationCreateModel? saved = null;
        var write = new Mock<ICustomerPaymentInformationWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<CustomerPaymentInformationCreateModel>(), It.IsAny<CancellationToken>()))
            .Callback<CustomerPaymentInformationCreateModel, CancellationToken>((model, _) => saved = model)
            .ReturnsAsync(Grid(9, isDefault: true));

        var read = new Mock<ICustomerPaymentInformationReadRepository>();
        read.Setup(r => r.FindCustomerProfileIdAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        var handler = new CreateCustomerPaymentInformationCommandHandler(
            settings.Object,
            cim.Object,
            read.Object,
            write.Object);

        var response = await handler.Handle(new CreateCustomerPaymentInformationCommand(CreateDto()), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.CardNumberMasked.Should().Be("**** **** **** 4242");
        saved.Should().NotBeNull();
        saved!.CustomerProfileId.Should().Be("profile-1");
        saved.CustomerPaymentProfileId.Should().Be("payment-1");
        saved.Last4.Should().Be("4242");
        saved.SetAsDefault.Should().BeTrue();
        cim.Verify(
            c => c.DeleteCustomerPaymentProfileAsync(
                It.IsAny<CompanyAuthorizeNetSettings>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_Returns404_WhenMerchantSettingsAreMissing()
    {
        var settings = new Mock<ICompanyAuthorizeNetSettings>();
        settings.Setup(s => s.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompanyAuthorizeNetSettingsResult.Missing());
        var cim = new Mock<IAuthorizeNetCustomerProfileClient>(MockBehavior.Strict);
        var handler = new CreateCustomerPaymentInformationCommandHandler(
            settings.Object,
            cim.Object,
            new Mock<ICustomerPaymentInformationReadRepository>().Object,
            new Mock<ICustomerPaymentInformationWriteService>().Object);

        var response = await handler.Handle(new CreateCustomerPaymentInformationCommand(CreateDto()), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Create_Returns400_WhenProviderRejectsTheCard()
    {
        var settings = new Mock<ICompanyAuthorizeNetSettings>();
        settings.Setup(s => s.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompanyAuthorizeNetSettingsResult.Found(Merchant()));
        var read = new Mock<ICustomerPaymentInformationReadRepository>();
        read.Setup(r => r.FindCustomerProfileIdAsync(42, It.IsAny<CancellationToken>())).ReturnsAsync("profile-1");
        var cim = new Mock<IAuthorizeNetCustomerProfileClient>();
        cim.Setup(c => c.CreateCustomerPaymentProfileAsync(
                It.IsAny<CompanyAuthorizeNetSettings>(),
                "profile-1",
                It.IsAny<AuthorizeNetOpaqueData>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AuthorizeNetCallResult(false, "Error", "E00027", "profile-1", null, null, null, null, null));
        var write = new Mock<ICustomerPaymentInformationWriteService>(MockBehavior.Strict);
        var handler = new CreateCustomerPaymentInformationCommandHandler(
            settings.Object,
            cim.Object,
            read.Object,
            write.Object);

        var response = await handler.Handle(new CreateCustomerPaymentInformationCommand(CreateDto()), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(400);
        response.Errors.Should().NotContain(error => error.Contains("E00027", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Delete_SoftDeletes_WhenProviderProfileIsNotFound()
    {
        var read = new Mock<ICustomerPaymentInformationReadRepository>();
        read.Setup(r => r.GetCardAsync(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Card(7, "profile-1", "payment-1"));
        var settings = new Mock<ICompanyAuthorizeNetSettings>();
        settings.Setup(s => s.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompanyAuthorizeNetSettingsResult.Found(Merchant()));
        var cim = new Mock<IAuthorizeNetCustomerProfileClient>();
        cim.Setup(c => c.DeleteCustomerPaymentProfileAsync(
                It.IsAny<CompanyAuthorizeNetSettings>(),
                "profile-1",
                "payment-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AuthorizeNetCallResult(false, "Error", "E00040", null, null, null, null, null, null));
        var write = new Mock<ICustomerPaymentInformationWriteService>();
        write.Setup(w => w.SoftDeleteAsync(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Grid(7, isActive: false));
        var handler = new DeleteCustomerPaymentInformationCommandHandler(
            settings.Object,
            cim.Object,
            read.Object,
            write.Object);

        var response = await handler.Handle(new DeleteCustomerPaymentInformationCommand(7), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
        write.Verify(w => w.SoftDeleteAsync(7, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_Returns404_WhenCardIsMissing()
    {
        var read = new Mock<ICustomerPaymentInformationReadRepository>();
        read.Setup(r => r.GetCardAsync(4, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CustomerPaymentInformationCard?)null);
        var handler = new GetCustomerPaymentInformationByIdQueryHandler(read.Object);

        var response = await handler.Handle(new GetCustomerPaymentInformationByIdQuery(4), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task List_RequiresArCustomerId()
    {
        var handler = new ListCustomerPaymentInformationQueryHandler(
            new Mock<ICustomerPaymentInformationReadRepository>(MockBehavior.Strict).Object);

        var response = await handler.Handle(
            new ListCustomerPaymentInformationQuery(0, new BillingListQuery()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(400);
    }

    private static CompanyAuthorizeNetSettings Merchant() =>
        new("login", "public", "transaction", true);

    private static CustomerPaymentInformationCreateDto CreateDto() =>
        new(42, new OpaquePaymentDataDto("COMMON.ACCEPT.INAPP.PAYMENT", "opaque-data-placeholder"), "Jane Cardholder", "12345", true);

    private static CustomerPaymentInformationCard Card(long id, string profileId, string paymentProfileId) =>
        new(id, 42, profileId, paymentProfileId, "Visa", "Jane Cardholder", "4242", 12, 2028, true, true);

    private static CustomerPaymentInformationGridDto Grid(long id, bool isDefault = false, bool isActive = true) =>
        new(id, "Visa", "Jane Cardholder", "**** **** **** 4242", "12/28", isDefault, isActive);
}

public sealed class CustomerPaymentInformationValidatorTests
{
    [Fact]
    public void List_RequiresArCustomerId()
    {
        var validator = new ListCustomerPaymentInformationQueryValidator();
        var result = validator.Validate(new ListCustomerPaymentInformationQuery(0, new BillingListQuery()));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorMessage == "arCustomerId is required.");
    }

    [Fact]
    public void Create_RequiresOpaqueDataAndCardholder()
    {
        var validator = new CreateCustomerPaymentInformationCommandValidator();
        var result = validator.Validate(
            new CreateCustomerPaymentInformationCommand(
                new CustomerPaymentInformationCreateDto(0, new OpaquePaymentDataDto("", ""), "", "", false)));

        result.IsValid.Should().BeFalse();
    }
}

public sealed class CustomerPaymentInformationWriteServiceTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task Create_SetAsDefault_ClearsPreviousActiveDefault()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        var first = await writeService.CreateAsync(Model(setAsDefault: true, profileId: "pay-1"), CancellationToken.None);
        var second = await writeService.CreateAsync(Model(setAsDefault: true, profileId: "pay-2"), CancellationToken.None);

        var rows = await context.FgsARCustomerPaymentInformations.AsNoTracking().OrderBy(e => e.Id).ToListAsync();
        rows.Should().HaveCount(2);
        rows[0].Id.Should().Be(first.Id);
        rows[0].IsDefault.Should().BeFalse();
        rows[1].Id.Should().Be(second.Id);
        rows[1].IsDefault.Should().BeTrue();
        rows.Should().OnlyContain(row => row.PaymentMethodType == "CARD" && row.CardType == "CREDIT" && row.IsActive);
    }

    [Fact]
    public async Task Read_ReturnsNull_ForAchAndMissingCards_AndReusesInactiveProfile()
    {
        var accessor = new DesignTimeTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        await using var context = await CreateContextAsync(accessor);
        var ach = Row(paymentMethod: "ACH", profileId: "ach-token", active: true);
        ach.CustomerProfileId = null;
        context.FgsARCustomerPaymentInformations.AddRange(
            ach,
            Row(paymentMethod: "CARD", profileId: "old-profile", active: false));
        await context.SaveChangesAsync();
        var achId = ach.Id;

        var read = new CustomerPaymentInformationReadRepository(context);
        (await read.GetCardAsync(achId, CancellationToken.None)).Should().BeNull();
        (await read.GetCardAsync(99, CancellationToken.None)).Should().BeNull();
        (await read.FindCustomerProfileIdAsync(42, CancellationToken.None)).Should().Be("old-profile");

        var page = await read.ListAsync(42, new BillingListQuery(), isActive: false, CancellationToken.None);
        page.Items.Should().ContainSingle(item => item.IsActive == false);
        page.ActiveCount.Should().Be(0);
    }

    private static CustomerPaymentInformationCreateModel Model(bool setAsDefault, string profileId) =>
        new(42, "profile-1", profileId, "Jane Cardholder", "Visa", "4242", 12, 2028, setAsDefault);

    private static FgsARCustomerPaymentInformation Row(string paymentMethod, string profileId, bool active) =>
        new()
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            ARCustomerId = 42,
            PaymentMethodType = paymentMethod,
            CardType = paymentMethod == "CARD" ? "CREDIT" : null,
            PaymentToken = profileId,
            CustomerProfileId = profileId,
            CustomerPaymentProfileId = profileId,
            CardHolderName = "Jane Cardholder",
            Last4 = "4242",
            IsActive = active,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = 0
        };

    private static CustomerPaymentInformationWriteService CreateWriteService(FgsBillingDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        var auditHelper = new BillingEntityAuditHelper(userContext.Object, tenantAccessor, new DateTimeProvider());
        return new CustomerPaymentInformationWriteService(
            context,
            new EfUnitOfWork<FgsBillingDbContext>(context),
            auditHelper);
    }

    private static async Task<FgsBillingDbContext> CreateContextAsync(ITenantContextAccessor? accessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsBillingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new FgsBillingDbContext(options, accessor ?? new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
