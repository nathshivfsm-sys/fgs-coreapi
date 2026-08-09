using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.GetFgsSetupPricingMatrixLaborTierById;

public sealed record GetFgsSetupPricingMatrixLaborTierByIdQuery(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>>;
