namespace UserService.Application.Signup.CreateCompanySignup;

public sealed record CompanySignupResponse(
    Guid TenantId,
    Guid UserId,
    Guid InviteId);
