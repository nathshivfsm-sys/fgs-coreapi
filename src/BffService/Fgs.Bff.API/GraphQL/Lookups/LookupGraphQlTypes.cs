using Fgs.Bff.Application.Features.Lookups;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using HotChocolate.Types;

namespace Fgs.Bff.API.GraphQL.Lookups;

/// <summary>GraphQL input mirroring <see cref="LookupRequestDto"/>.</summary>
public sealed class LookupRequestInput
{
    public LookupKey Key { get; set; }
    public bool ActiveOnly { get; set; } = true;
    public string? CountryCode { get; set; }
    public string? UnitType { get; set; }
    public int? SourceTypeId { get; set; }
    public long? JobTypeId { get; set; }
    public long? PriceBookId { get; set; }
    public long? PricingMatrixId { get; set; }
    public long? PricingMatrixLaborId { get; set; }
    public long? UniversalPricingServiceId { get; set; }
    public long? InventoryItemId { get; set; }
    public long? FgsRoleId { get; set; }
    public long? RoleId { get; set; }
    public Guid? UserId { get; set; }
    public bool? ShowToFieldTech { get; set; }
    public bool? AllowToPick { get; set; }
    public bool? IsMobileVisible { get; set; }
    public bool? IsCustomerPortalVisible { get; set; }
    public string? StateProvinceCode { get; set; }

    public LookupRequestDto ToDto() =>
        new(
            Key,
            ActiveOnly,
            CountryCode,
            UnitType,
            SourceTypeId,
            JobTypeId,
            PriceBookId,
            PricingMatrixId,
            PricingMatrixLaborId,
            UniversalPricingServiceId,
            InventoryItemId,
            FgsRoleId,
            RoleId,
            UserId,
            ShowToFieldTech,
            AllowToPick,
            IsMobileVisible,
            IsCustomerPortalVisible,
            StateProvinceCode);
}

public sealed class LookupItemType : ObjectType<LookupItemDto>
{
    protected override void Configure(IObjectTypeDescriptor<LookupItemDto> descriptor)
    {
        descriptor.Field(x => x.Extra).Type<AnyType>();
    }
}
