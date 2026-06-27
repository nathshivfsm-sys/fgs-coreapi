using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.LookupTechTrades;

public sealed record LookupTechTradesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<TechTradeLookupDto>>>;
