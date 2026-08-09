using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Signup;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Bff.Application.Features.Signup.Commands.CreateCompanySignup;

/// <summary>
/// BFF orchestration: validate → User identity signup → Setup business types → consolidated response.
/// On Setup failure after identity creation, marks the tenant as ProvisioningFailed for operator remediation.
/// </summary>
public sealed class CreateCompanySignupCommandHandler(
    IUserSignupClient userSignupClient,
    IUserTenantClient userTenantClient,
    ISetupClient setupClient,
    ILogger<CreateCompanySignupCommandHandler> logger)
    : IRequestHandler<CreateCompanySignupCommand, ApiResponse<CompanySignupResultDto>>
{
    public async Task<ApiResponse<CompanySignupResultDto>> Handle(
        CreateCompanySignupCommand request,
        CancellationToken cancellationToken)
    {
        var identityRequest = new CompanySignupRequest(
            request.Contact,
            request.Company,
            request.BusinessTypeIds,
            request.TimeZone,
            request.DefaultCurrency,
            request.AuthenticationMethod);

        var identityResponse = await userSignupClient.CreateCompanySignupAsync(
            identityRequest,
            cancellationToken);

        if (!identityResponse.Success || identityResponse.Data is null)
        {
            return ApiResponse<CompanySignupResultDto>.Fail(
                identityResponse.Errors.Count > 0
                    ? identityResponse.Errors
                    : ["Company signup failed in User service."],
                identityResponse.StatusCode is > 0
                    ? identityResponse.StatusCode
                    : ApiStatusCodes.BadRequest);
        }

        var identity = identityResponse.Data;
        var businessTypeIds = request.BusinessTypeIds.Distinct().ToList();

        try
        {
            var setupResponse = await setupClient.AddCompanyBusinessTypesAsync(
                identity.TenantId,
                identity.CompanyNumber,
                new AddCompanyBusinessTypesRequest(
                    businessTypeIds,
                    identity.CompanyGuid,
                    identity.TenantCode,
                    request.Company.Name.Trim()),
                cancellationToken);

            if (!setupResponse.Success)
            {
                await TryMarkProvisioningFailedAsync(identity.TenantId, cancellationToken);

                logger.LogError(
                    "Company signup identity created (TenantId={TenantId}, CompanyNumber={CompanyNumber}) but Setup business-type seeding failed: {Errors}",
                    identity.TenantId,
                    identity.CompanyNumber,
                    string.Join("; ", setupResponse.Errors));

                var errors = setupResponse.Errors.Count > 0
                    ? setupResponse.Errors.ToList()
                    : ["Business type seeding failed after identity was created."];
                errors.Add(
                    $"Identity was created (tenantId={identity.TenantId}, companyNumber={identity.CompanyNumber}). Tenant marked ProvisioningFailed; retry business-type seeding or contact support.");

                return ApiResponse<CompanySignupResultDto>.Fail(
                    errors,
                    setupResponse.StatusCode is ApiStatusCodes.Unauthorized or ApiStatusCodes.BadRequest
                        ? setupResponse.StatusCode
                        : ApiStatusCodes.InternalServerError);
            }
        }
        catch (Exception ex)
        {
            await TryMarkProvisioningFailedAsync(identity.TenantId, cancellationToken);

            logger.LogError(
                ex,
                "Company signup identity created (TenantId={TenantId}, CompanyNumber={CompanyNumber}) but Setup call threw.",
                identity.TenantId,
                identity.CompanyNumber);

            return ApiResponse<CompanySignupResultDto>.Fail(
                [
                    "Business type seeding failed after identity was created.",
                    ex.Message,
                    $"Identity was created (tenantId={identity.TenantId}, companyNumber={identity.CompanyNumber}). Tenant marked ProvisioningFailed; retry business-type seeding or contact support."
                ],
                ApiStatusCodes.InternalServerError);
        }

        return ApiResponse<CompanySignupResultDto>.Ok(identity, ApiStatusCodes.Created);
    }

    private async Task TryMarkProvisioningFailedAsync(long tenantId, CancellationToken cancellationToken)
    {
        try
        {
            await userTenantClient.UpdateStatusAsync(
                tenantId,
                new UpdateTenantStatusRequest(TenantStatusIds.ProvisioningFailed),
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "Failed to mark tenant {TenantId} as ProvisioningFailed after Setup signup failure.",
                tenantId);
        }
    }
}
