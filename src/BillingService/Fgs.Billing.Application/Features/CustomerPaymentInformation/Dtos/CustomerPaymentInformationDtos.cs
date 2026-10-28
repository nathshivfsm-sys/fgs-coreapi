namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;

public sealed record OpaquePaymentDataDto(
    string DataDescriptor,
    string DataValue);

public sealed record CustomerPaymentInformationCreateDto(
    long ArCustomerId,
    OpaquePaymentDataDto OpaqueData,
    string CardHolderName,
    string BillingZip,
    bool SetAsDefault);

public sealed record CustomerPaymentInformationUpdateDto(
    string CardHolderName,
    short ExpirationMonth,
    short ExpirationYear,
    bool IsDefault,
    OpaquePaymentDataDto? OpaqueData);

public sealed record CustomerPaymentInformationGridDto(
    long Id,
    string? CardBrand,
    string? CardHolderName,
    string CardNumberMasked,
    string? Expiry,
    bool IsDefault,
    bool IsActive);

public sealed record CustomerPaymentInformationListDto(
    IReadOnlyList<CustomerPaymentInformationGridDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int ActiveCount);

public sealed record CustomerPaymentInformationAcceptConfigDto(
    string ApiLoginId,
    string PublicClientKey,
    string AcceptJsUrl);

public sealed record CustomerPaymentInformationCard(
    long Id,
    long ArCustomerId,
    string? CustomerProfileId,
    string? CustomerPaymentProfileId,
    string? CardBrand,
    string? CardHolderName,
    string? Last4,
    short? ExpirationMonth,
    short? ExpirationYear,
    bool IsDefault,
    bool IsActive);

public sealed record CustomerPaymentInformationCreateModel(
    long ArCustomerId,
    string CustomerProfileId,
    string CustomerPaymentProfileId,
    string CardHolderName,
    string? CardBrand,
    string Last4,
    short ExpirationMonth,
    short ExpirationYear,
    bool SetAsDefault);

public sealed record CustomerPaymentInformationUpdateModel(
    string CardHolderName,
    string? CardBrand,
    string? Last4,
    short ExpirationMonth,
    short ExpirationYear,
    bool IsDefault);
