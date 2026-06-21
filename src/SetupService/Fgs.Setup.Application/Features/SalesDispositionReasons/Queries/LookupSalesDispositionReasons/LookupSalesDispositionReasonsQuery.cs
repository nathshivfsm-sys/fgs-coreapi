using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.LookupSalesDispositionReasons;

public sealed record LookupSalesDispositionReasonsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSalesDispositionReasonLookupDto>>>;
