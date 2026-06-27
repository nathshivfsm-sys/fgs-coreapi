using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.LookupTags;

public sealed record LookupTagsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsTagLookupDto>>>;
