using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.LookupSetupDescriptions;

public sealed record LookupSetupDescriptionsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupDescriptionLookupDto>>>;
