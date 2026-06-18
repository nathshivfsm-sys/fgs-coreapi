using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;

namespace Fgs.Setup.Infrastructure.GLBreaks;

internal sealed class GLBreakSummaryRow
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? BreakLabel { get; set; }

    public short BreakLevel { get; set; }

    public long? LogoFileId { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public GLBreakSummaryDto ToDto() =>
        new(Id, TenantId, CompanyId, Code, Name, BreakLabel, BreakLevel, LogoFileId, IsActive, CreatedOn, UpdatedOn);
}

internal sealed class GLBreakDetailRow
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? BreakLabel { get; set; }

    public short BreakLevel { get; set; }

    public long? LogoFileId { get; set; }

    public Guid? AddressId { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public Guid? LocationId { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? AddressLine3 { get; set; }

    public string? AddressLine4 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? County { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? FormattedAddress { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? PlaceId { get; set; }

    public bool? LocationIsActive { get; set; }

    public DateTimeOffset? LocationCreatedOn { get; set; }

    public string? LocationCreatedBy { get; set; }

    public DateTimeOffset? LocationUpdatedOn { get; set; }

    public string? LocationUpdatedBy { get; set; }

    public GLBreakDetailDto ToDto(IReadOnlyList<GLBreakTradeDto> trades) =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            BreakLabel,
            BreakLevel,
            LogoFileId,
            ToLocationDto(),
            trades,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);

    private LocationDetailDto? ToLocationDto()
    {
        if (LocationId is not Guid locationId)
        {
            return null;
        }

        return new LocationDetailDto(
            locationId,
            AddressLine1,
            AddressLine2,
            AddressLine3,
            AddressLine4,
            City,
            State,
            County,
            Country,
            PostalCode,
            FormattedAddress,
            Latitude,
            Longitude,
            PlaceId,
            LocationIsActive ?? true,
            LocationCreatedOn ?? default,
            LocationCreatedBy,
            LocationUpdatedOn,
            LocationUpdatedBy);
    }
}

internal sealed class GLBreakLookupRow
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short BreakLevel { get; set; }

    public GLBreakLookupDto ToDto() => new(Id, Code, Name, BreakLevel);
}

internal sealed class GLBreakTradeRow
{
    public long Id { get; set; }

    public long GLBreakId { get; set; }

    public string TradeCode { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public GLBreakTradeDto ToDto() => new(Id, GLBreakId, TradeCode, CreatedOn, CreatedBy);
}
