using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.LookupSalesActivityTypes;

public sealed record LookupSalesActivityTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>>;
