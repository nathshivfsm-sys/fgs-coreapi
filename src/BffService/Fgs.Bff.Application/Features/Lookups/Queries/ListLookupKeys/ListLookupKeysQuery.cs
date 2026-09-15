using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Bff.Application.Features.Lookups.Queries.ListLookupKeys;

public sealed record ListLookupKeysQuery : IRequest<ApiResponse<IReadOnlyList<LookupKeyInfoDto>>>;
