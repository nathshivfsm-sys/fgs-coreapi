namespace Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;

public sealed record CompanyAuthorizeNetSettings(
    string ApiLoginId,
    string PublicClientKey,
    string TransactionKey,
    bool UseSandbox);

public sealed record CompanyAuthorizeNetSettingsResult(
    CompanyAuthorizeNetSettings? Settings,
    bool TransportFailure)
{
    public static CompanyAuthorizeNetSettingsResult Found(CompanyAuthorizeNetSettings settings) =>
        new(settings, false);

    public static CompanyAuthorizeNetSettingsResult Missing() =>
        new(null, false);

    public static CompanyAuthorizeNetSettingsResult Unavailable() =>
        new(null, true);
}

public interface ICompanyAuthorizeNetSettings
{
    Task<CompanyAuthorizeNetSettingsResult> GetAsync(CancellationToken cancellationToken = default);
}

public sealed record AuthorizeNetOpaqueData(string DataDescriptor, string DataValue);

public sealed record AuthorizeNetCallResult(
    bool TransportFailure,
    string ResultCode,
    string? MessageCode,
    string? CustomerProfileId,
    string? CustomerPaymentProfileId,
    string? CardBrand,
    string? Last4,
    short? ExpirationMonth,
    short? ExpirationYear)
{
    public bool IsOk =>
        !TransportFailure
        && string.Equals(ResultCode, "Ok", StringComparison.OrdinalIgnoreCase);

    public bool IsDuplicate =>
        string.Equals(MessageCode, "E00039", StringComparison.OrdinalIgnoreCase);

    public bool IsProfileNotFound =>
        string.Equals(MessageCode, "E00040", StringComparison.OrdinalIgnoreCase);

    public static AuthorizeNetCallResult Transport() =>
        new(true, "Error", null, null, null, null, null, null, null);

    public static AuthorizeNetCallResult Ok(
        string? customerProfileId,
        string? customerPaymentProfileId,
        string? cardBrand = null,
        string? last4 = null,
        short? expirationMonth = null,
        short? expirationYear = null) =>
        new(
            false,
            "Ok",
            "I00001",
            customerProfileId,
            customerPaymentProfileId,
            cardBrand,
            last4,
            expirationMonth,
            expirationYear);
}

public interface IAuthorizeNetCustomerProfileClient
{
    Task<AuthorizeNetCallResult> CreateCustomerProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string merchantCustomerId,
        CancellationToken cancellationToken = default);

    Task<AuthorizeNetCallResult> GetCustomerProfileByMerchantCustomerIdAsync(
        CompanyAuthorizeNetSettings settings,
        string merchantCustomerId,
        CancellationToken cancellationToken = default);

    Task<AuthorizeNetCallResult> CreateCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        AuthorizeNetOpaqueData opaqueData,
        string cardHolderName,
        string billingZip,
        CancellationToken cancellationToken = default);

    Task<AuthorizeNetCallResult> GetCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        string customerPaymentProfileId,
        CancellationToken cancellationToken = default);

    Task<AuthorizeNetCallResult> UpdateCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        string customerPaymentProfileId,
        string cardHolderName,
        short expirationMonth,
        short expirationYear,
        string? last4,
        AuthorizeNetOpaqueData? opaqueData,
        CancellationToken cancellationToken = default);

    Task<AuthorizeNetCallResult> DeleteCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        string customerPaymentProfileId,
        CancellationToken cancellationToken = default);
}
