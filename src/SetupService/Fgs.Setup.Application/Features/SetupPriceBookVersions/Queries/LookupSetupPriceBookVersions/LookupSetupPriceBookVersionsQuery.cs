using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.LookupSetupPriceBookVersions;

public sealed record LookupSetupPriceBookVersionsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupPriceBookVersionLookupDto>>>;
