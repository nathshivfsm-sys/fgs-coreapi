using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Queries.LookupPriceBooks;

public sealed record LookupPriceBooksQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsPriceBookLookupDto>>>;
