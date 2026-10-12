using Fgs.Crm.Domain.Enums;

namespace Fgs.Crm.Application.Features.Customers.Dtos;

public enum CrmCustomerCreationOption
{
    BillToOnly = 0,
    BillToAndServiceLocation = 1
}

public sealed record CrmCustomerSummaryDto(
    long Id,
    string CustomerNumber,
    string Name,
    string DisplayName,
    string? City,
    string? State,
    string? PostalCode,
    string? Country,
    string? CustomerAccountNumber,
    bool IsActive,
    string? FormattedAddress = null,
    string? PrimaryContactName = null,
    string? Email = null,
    string? Phone = null,
    IReadOnlyList<long>? TagIds = null,
    DateTimeOffset CreatedOn = default,
    bool IsPreferredCustomer = false);

public sealed record CrmCustomerDetailDto(
    long Id,
    string CustomerNumber,
    string Name,
    string DisplayName,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? County,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude,
    string? PlaceId,
    long? DefaultPaymentTermId,
    long? DefaultMaterialPricingMatrixId,
    long? DefaultLaborPricingMatrixId,
    long? DefaultOtherPricingMatrixId,
    bool DefaultPORequired,
    bool TaxExempt,
    string? TaxExemptNumber,
    string? CustomerAccountNumber,
    string? ExternalEntityId,
    string? ExternalVersion,
    bool IsActive,
    string? Website = null,
    string? PrimaryContactName = null,
    string? PrimaryContactEmail = null,
    string? PrimaryContactPhone = null,
    DateTimeOffset CreatedOn = default,
    IReadOnlyList<long>? TagIds = null);

public sealed record CrmCustomerLookupDto(
    long Id,
    string CustomerNumber,
    string DisplayName);

public interface ICrmServiceLocationInput
{
    ServiceLocationType ServiceLocationType { get; }

    string? Name { get; }

    string? DisplayName { get; }

    string? AddressLine1 { get; }

    string? AddressLine2 { get; }

    string? AddressLine3 { get; }

    string? AddressLine4 { get; }

    string? City { get; }

    string? State { get; }

    string? County { get; }

    string? Country { get; }

    string? PostalCode { get; }

    string? FormattedAddress { get; }

    decimal? Latitude { get; }

    decimal? Longitude { get; }

    string? PlaceId { get; }

    bool? TaxExempt { get; }

    string? PrimaryContactName { get; }

    string? PrimaryContactEmail { get; }

    string? PrimaryContactPhone { get; }

    bool? CanReceiveEstimates { get; }

    bool? CanReceiveInvoices { get; }

    bool? CanReceiveAppointments { get; }

    IReadOnlyList<long>? TagIds { get; }

    CustomerType? CustomerType { get; }
}

public sealed record CrmServiceLocationCreateDto(
    ServiceLocationType ServiceLocationType,
    string? Name = null,
    string? DisplayName = null,
    string? AddressLine1 = null,
    string? AddressLine2 = null,
    string? AddressLine3 = null,
    string? AddressLine4 = null,
    string? City = null,
    string? State = null,
    string? County = null,
    string? Country = null,
    string? PostalCode = null,
    string? FormattedAddress = null,
    decimal? Latitude = null,
    decimal? Longitude = null,
    string? PlaceId = null,
    bool? TaxExempt = null,
    string? PrimaryContactName = null,
    string? PrimaryContactEmail = null,
    string? PrimaryContactPhone = null,
    bool? CanReceiveEstimates = null,
    bool? CanReceiveInvoices = null,
    bool? CanReceiveAppointments = null,
    IReadOnlyList<long>? TagIds = null,
    CustomerType? CustomerType = null) : ICrmServiceLocationInput;

public sealed record CrmServiceLocationUpdateDto(
    ServiceLocationType ServiceLocationType,
    string? Name = null,
    string? DisplayName = null,
    string? AddressLine1 = null,
    string? AddressLine2 = null,
    string? AddressLine3 = null,
    string? AddressLine4 = null,
    string? City = null,
    string? State = null,
    string? County = null,
    string? Country = null,
    string? PostalCode = null,
    string? FormattedAddress = null,
    decimal? Latitude = null,
    decimal? Longitude = null,
    string? PlaceId = null,
    bool? TaxExempt = null,
    string? PrimaryContactName = null,
    string? PrimaryContactEmail = null,
    string? PrimaryContactPhone = null,
    bool? CanReceiveEstimates = null,
    bool? CanReceiveInvoices = null,
    bool? CanReceiveAppointments = null,
    IReadOnlyList<long>? TagIds = null,
    CustomerType? CustomerType = null,
    bool IsActive = false) : ICrmServiceLocationInput;

public sealed record CrmServiceLocationDetailDto(
    long Id,
    long CustomerId,
    int LocationSequence,
    string LocationNumber,
    bool IsActive,
    ServiceLocationType ServiceLocationType,
    string Name,
    string DisplayName,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? County,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude,
    string? PlaceId,
    bool TaxExempt,
    string? PrimaryContactName,
    string? PrimaryContactEmail,
    string? PrimaryContactPhone,
    bool CanReceiveEstimates,
    bool CanReceiveInvoices,
    bool CanReceiveAppointments,
    IReadOnlyList<long> TagIds,
    CustomerType? CustomerType);

public sealed record CrmServiceLocationSummaryDto(
    long Id,
    string LocationNumber,
    string Name,
    string DisplayName,
    string? FormattedAddress,
    string? Email,
    string? Phone,
    bool IsActive,
    CustomerType? CustomerType = null);

/// <summary>
/// Active and inactive location counts for one customer. Not narrowed by search or the selected tab.
/// </summary>
public sealed record CrmServiceLocationListSummaryDto(
    int ActiveCount,
    int InactiveCount);

/// <summary>
/// List page plus that customer's location summary. <see cref="TotalCount"/> respects isActive and search;
/// <see cref="Summary"/> ignores search (zeros when includeSummary is false).
/// </summary>
public sealed record CrmServiceLocationListResultDto(
    IReadOnlyList<CrmServiceLocationSummaryDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    CrmServiceLocationListSummaryDto Summary);

public sealed record CrmServiceLocationCreatedDto(
    long ServiceLocationId,
    long CustomerId,
    int LocationSequence,
    string LocationNumber);

public sealed record CrmCustomerCreateResultDto(
    CrmCustomerDetailDto Customer,
    long? ServiceLocationId);

/// <summary>
/// Company-scoped aggregate counts for the customer list. Not narrowed by search, tab, or preferred filters.
/// </summary>
public sealed record CrmCustomerListSummaryDto(
    int TotalCustomers,
    int ActiveCustomers,
    int InactiveCustomers,
    int NewThisMonth,
    int PreviousMonthNewCustomers,
    decimal? NewCustomerPercentChange);

/// <summary>
/// List page plus company summary. <see cref="TotalCount"/> respects current list filters;
/// <see cref="Summary"/> is company-wide (zeros when includeSummary is false).
/// </summary>
public sealed record CrmCustomerListResultDto(
    IReadOnlyList<CrmCustomerSummaryDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    CrmCustomerListSummaryDto Summary);

public sealed record CrmCustomerCreateDto(
    string CustomerNumber,
    string Name,
    string DisplayName,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? County,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude,
    string? PlaceId,
    long? DefaultPaymentTermId,
    long? DefaultMaterialPricingMatrixId,
    long? DefaultLaborPricingMatrixId,
    long? DefaultOtherPricingMatrixId,
    bool DefaultPORequired,
    bool TaxExempt,
    string? TaxExemptNumber,
    string? CustomerAccountNumber,
    string? ExternalEntityId,
    string? ExternalVersion,
    string? Website = null,
    string? PrimaryContactName = null,
    string? PrimaryContactEmail = null,
    string? PrimaryContactPhone = null,
    CrmCustomerCreationOption CreationOption = CrmCustomerCreationOption.BillToOnly,
    CrmServiceLocationCreateDto? ServiceLocation = null,
    IReadOnlyList<long>? TagIds = null);

public sealed record CrmCustomerUpdateDto(
    string CustomerNumber,
    string Name,
    string DisplayName,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? County,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude,
    string? PlaceId,
    long? DefaultPaymentTermId,
    long? DefaultMaterialPricingMatrixId,
    long? DefaultLaborPricingMatrixId,
    long? DefaultOtherPricingMatrixId,
    bool DefaultPORequired,
    bool TaxExempt,
    string? TaxExemptNumber,
    string? CustomerAccountNumber,
    string? ExternalEntityId,
    string? ExternalVersion,
    string? Website = null);

public sealed record CrmCustomerPatchDto(
    string? CustomerNumber,
    string? Name,
    string? DisplayName,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? County,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude,
    string? PlaceId,
    long? DefaultPaymentTermId,
    long? DefaultMaterialPricingMatrixId,
    long? DefaultLaborPricingMatrixId,
    long? DefaultOtherPricingMatrixId,
    bool? DefaultPORequired,
    bool? TaxExempt,
    string? TaxExemptNumber,
    string? CustomerAccountNumber,
    string? ExternalEntityId,
    string? ExternalVersion,
    bool? IsActive,
    string? Website = null);

public sealed record CrmCustomerListFilters(
    string? CustomerNumber = null,
    string? Name = null,
    string? DisplayName = null,
    bool? IsPreferred = null);
