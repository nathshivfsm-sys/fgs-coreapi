using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Queries.LookupFgsApiEvents;

public sealed record LookupFgsApiEventsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsApiEventLookupDto>>>;
