using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Queries.LookupPriceBookItems;

public sealed record LookupPriceBookItemsQuery(long? PriceBookId = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsPriceBookItemLookupDto>>>;
