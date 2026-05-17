using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Signup;

public sealed class SignupUniquenessValidator : ISignupUniquenessValidator
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailNormalizer _emailNormalizer;
    private readonly IDateTimeProvider _dateTime;

    public SignupUniquenessValidator(
        IUnitOfWork unitOfWork,
        IEmailNormalizer emailNormalizer,
        IDateTimeProvider dateTime)
    {
        _unitOfWork = unitOfWork;
        _emailNormalizer = emailNormalizer;
        _dateTime = dateTime;
    }

    public async Task<IReadOnlyList<string>> ValidateAsync(
        CreateCompanySignupCommand command,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();
        var companyName = command.Company.Name.Trim();
        var normalizedCompanyName = NormalizeName(companyName);
        var normalizedEmail = _emailNormalizer.Normalize(command.Contact.Email);
        var phoneNumber = command.Contact.PhoneNumber.Trim();

        var tenantRepo = _unitOfWork.Repository<FgsTenant>();
        var companyRepo = _unitOfWork.Repository<FgsTenantCompany>();
        var userRepo = _unitOfWork.Repository<FgsUser>();
        var invitationRepo = _unitOfWork.Repository<FgsInvitation>();

        if (await CompanyNameExistsAsync(tenantRepo, companyRepo, normalizedCompanyName, cancellationToken))
        {
            errors.Add("A company with this name is already registered.");
        }

        if (await EmailExistsAsync(userRepo, invitationRepo, normalizedEmail, cancellationToken))
        {
            errors.Add("This email address is already associated with an account or pending invitation.");
        }

        if (await PhoneNumberExistsAsync(tenantRepo, companyRepo, phoneNumber, cancellationToken))
        {
            errors.Add("This phone number is already associated with a registered company.");
        }

        return errors;
    }

    private async Task<bool> CompanyNameExistsAsync(
        IRepository<FgsTenant> tenantRepo,
        IRepository<FgsTenantCompany> companyRepo,
        string normalizedCompanyName,
        CancellationToken cancellationToken)
    {
        if (await tenantRepo.AnyAsync(
                t => t.IsActive && t.Name.ToUpper() == normalizedCompanyName,
                cancellationToken))
        {
            return true;
        }

        return await companyRepo.AnyAsync(
            c => c.IsActive && c.Name.ToUpper() == normalizedCompanyName,
            cancellationToken);
    }

    private async Task<bool> EmailExistsAsync(
        IRepository<FgsUser> userRepo,
        IRepository<FgsInvitation> invitationRepo,
        string normalizedEmail,
        CancellationToken cancellationToken)
    {
        if (await userRepo.AnyAsync(
                u => !u.IsDeleted && u.NormalizedEmail == normalizedEmail,
                cancellationToken))
        {
            return true;
        }

        var now = _dateTime.UtcNow;
        var invitations = await invitationRepo.ListAsync(
            i => !i.IsDeleted
                && i.Status == InvitationStatus.Pending
                && i.ExpiresAtUtc > now,
            cancellationToken);

        return invitations.Any(i => _emailNormalizer.Normalize(i.Email) == normalizedEmail);
    }

    private static async Task<bool> PhoneNumberExistsAsync(
        IRepository<FgsTenant> tenantRepo,
        IRepository<FgsTenantCompany> companyRepo,
        string phoneNumber,
        CancellationToken cancellationToken)
    {
        if (await tenantRepo.AnyAsync(
                t => t.IsActive && t.PhoneNumber != null && t.PhoneNumber == phoneNumber,
                cancellationToken))
        {
            return true;
        }

        return await companyRepo.AnyAsync(
            c => c.IsActive && c.PhoneNumber != null && c.PhoneNumber == phoneNumber,
            cancellationToken);
    }

    private static string NormalizeName(string value) => value.Trim().ToUpperInvariant();
}
