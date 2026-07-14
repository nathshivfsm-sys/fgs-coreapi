using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Queries.LookupFgsDataAccesses;

public sealed record LookupFgsDataAccessesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsDataAccessLookupDto>>>;
