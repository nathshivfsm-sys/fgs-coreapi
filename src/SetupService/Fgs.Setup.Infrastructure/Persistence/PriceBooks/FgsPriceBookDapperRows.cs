using Fgs.Setup.Application.Features.PriceBooks.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.PriceBooks;

internal sealed class FgsPriceBookSummaryRow
{
    public long Id { get; set; }
    public string PriceBookCode { get; set; } = null!;
    public string PriceBookName { get; set; } = null!;
    public long JobTypeId { get; set; }
    public string PricingModel { get; set; } = null!;
    public int EstimatedDurationMinutes { get; set; }
    public decimal? BasePrice { get; set; }
    public bool IsTaxable { get; set; }
    public bool IsActive { get; set; }

    public FgsPriceBookSummaryDto ToDto() =>
        new(Id, PriceBookCode, PriceBookName, JobTypeId, PricingModel, EstimatedDurationMinutes, BasePrice, IsTaxable, IsActive);
}

internal sealed class FgsPriceBookDetailRow
{
    public long Id { get; set; }
    public string PriceBookCode { get; set; } = null!;
    public string PriceBookName { get; set; } = null!;
    public string? Description { get; set; }
    public long JobTypeId { get; set; }
    public string PricingModel { get; set; } = null!;
    public int EstimatedDurationMinutes { get; set; }
    public decimal? BasePrice { get; set; }
    public bool IsTaxable { get; set; }
    public bool IsActive { get; set; }

    public FgsPriceBookDetailDto ToDto() =>
        new(Id, PriceBookCode, PriceBookName, Description, JobTypeId, PricingModel, EstimatedDurationMinutes, BasePrice, IsTaxable, IsActive);
}

internal sealed class FgsPriceBookLookupRow
{
    public long Id { get; set; }
    public string PriceBookCode { get; set; } = null!;
    public string PriceBookName { get; set; } = null!;
    public string PricingModel { get; set; } = null!;

    public FgsPriceBookLookupDto ToDto() =>
        new(Id, PriceBookCode, PriceBookName, PricingModel);
}
