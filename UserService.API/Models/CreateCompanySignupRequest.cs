namespace UserService.API.Models;

public sealed class CreateCompanySignupRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
}
