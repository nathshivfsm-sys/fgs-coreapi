using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Features.Customers.Validators;
using Moq;

namespace Fgs.Crm.Tests.Customers;

public sealed class CrmCustomerValidatorTests
{
    private readonly Mock<ICrmCustomerReadRepository> _readRepository = new();

    private static CrmCustomerCreateDto SampleCreateDto(string customerNumber = "CUST01") =>
        new(
            customerNumber,
            "Acme Corporation",
            "Acme Corp",
            "100 Main St",
            null,
            null,
            null,
            "Austin",
            "TX",
            null,
            "US",
            "78701",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            null,
            null,
            null,
            null);

    [Fact]
    public async Task CreateValidator_WhenCustomerNumberMissing_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(SampleCreateDto("") with { CustomerNumber = "" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CustomerNumber");
    }

    [Fact]
    public async Task CreateValidator_WhenCustomerNumberNotUppercase_HasValidationError()
    {
        var validator = new CreateCrmCustomerCommandValidator(_readRepository.Object);
        var command = new CreateCrmCustomerCommand(SampleCreateDto("cust01"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CustomerNumber");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateNumberExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByCustomerNumberAsync("CUST01", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateCrmCustomerCommandValidator(_readRepository.Object);
        var updateDto = new CrmCustomerUpdateDto(
            "CUST01",
            "Acme Corporation",
            "Acme Corp",
            "100 Main St",
            null,
            null,
            null,
            "Austin",
            "TX",
            null,
            "US",
            "78701",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            null,
            null,
            null,
            null);

        var result = await validator.ValidateAsync(new UpdateCrmCustomerCommand(5, updateDto));

        result.IsValid.Should().BeTrue();
    }
}
