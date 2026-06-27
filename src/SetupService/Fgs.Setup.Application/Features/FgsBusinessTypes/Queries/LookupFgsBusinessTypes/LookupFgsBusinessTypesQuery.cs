using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.LookupFgsBusinessTypes;

public sealed record LookupFgsBusinessTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsBusinessTypeLookupDto>>>;
