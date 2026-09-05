using Fgs.User.Application.Features.Companies.Commands.CreateCompany;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Validators;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Application.Features.Tenants.Validators;

namespace Fgs.User.Tests.Application;

public sealed class TenantCompanyValidatorTests
{
    [Fact]
    public async Task UpdateTenant_RequiresName()
    {
        var validator = new UpdateTenantCommandValidator();
        var dto = new TenantUpdateDto("", null, null, null, null, null, null, true);
        var result = await validator.ValidateAsync(new UpdateTenantCommand(1, dto));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateCompany_RequiresCodeAndName()
    {
        var validator = new CreateCompanyCommandValidator();
        var dto = new CompanyCreateDto("", "", null, null, null, null, null, null, null, null, null);
        var result = await validator.ValidateAsync(new CreateCompanyCommand(1, dto));
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Code"));
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Name"));
    }

    [Fact]
    public async Task CreateCompany_AcceptsValidPayload()
    {
        var validator = new CreateCompanyCommandValidator();
        var dto = new CompanyCreateDto(
            "BRANCH2",
            "Branch Two",
            null,
            "a@b.com",
            null,
            null,
            null,
            null,
            null,
            null,
            null);
        var result = await validator.ValidateAsync(new CreateCompanyCommand(1, dto));
        result.IsValid.Should().BeTrue();
    }
}
