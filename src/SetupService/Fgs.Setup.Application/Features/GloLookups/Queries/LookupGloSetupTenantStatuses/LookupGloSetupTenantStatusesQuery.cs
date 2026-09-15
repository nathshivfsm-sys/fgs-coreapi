using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloSetupTenantStatuses;

public sealed record LookupGloSetupTenantStatusesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloSetupTenantStatusLookupDto>>>;
