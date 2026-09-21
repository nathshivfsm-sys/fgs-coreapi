using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.GetUserListEnrichment;

public sealed record GetUserListEnrichmentQuery(IReadOnlyList<Guid> UserIds)
    : IRequest<ApiResponse<IReadOnlyList<FgsUserListEnrichmentDto>>>;
