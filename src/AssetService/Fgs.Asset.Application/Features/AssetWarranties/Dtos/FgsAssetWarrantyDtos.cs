namespace Fgs.Asset.Application.Features.AssetWarranties.Dtos;
public sealed record FgsAssetWarrantySummaryDto(long Id, long AssetId, string WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly StartDate, DateOnly EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyDetailDto(long Id, long AssetId, string WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly StartDate, DateOnly EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyLookupDto(long Id, string WarrantyType, DateOnly StartDate, DateOnly EndDate);
public sealed record FgsAssetWarrantyCreateDto(long AssetId, string WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly StartDate, DateOnly EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyUpdateDto(long AssetId, string WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly StartDate, DateOnly EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyPatchDto(long? AssetId, string? WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly? StartDate, DateOnly? EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyListFilters(long? AssetId = null, string? WarrantyType = null);
