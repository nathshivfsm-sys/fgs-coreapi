using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Commands.CreateFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Commands.UpdateFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Billing.Application.Features.Invoices.Validators;
using Moq;

namespace Fgs.Billing.Tests.Invoices;

public sealed class FgsInvoiceValidatorTests
{
    private readonly Mock<IFgsInvoiceReadRepository> _readRepository = new();

    private static FgsInvoiceCreateDto SampleCreateDto(string invoiceNumber = "INV-001") =>
        new(
            invoiceNumber,
            1,
            100,
            200,
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
            null,
            null,
            new DateOnly(2026, 8, 1),
            new DateOnly(2026, 8, 1),
            new DateOnly(2026, 8, 31),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            100m,
            0m,
            100m,
            8m,
            108m,
            108m);

    [Fact]
    public async Task CreateValidator_WhenInvoiceNumberMissing_HasValidationError()
    {
        var validator = new CreateFgsInvoiceCommandValidator(_readRepository.Object);
        var command = new CreateFgsInvoiceCommand(SampleCreateDto("") with { InvoiceNumber = "" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.InvoiceNumber");
    }

    [Fact]
    public async Task CreateValidator_WhenInvoiceNumberNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsInvoiceCommandValidator(_readRepository.Object);
        var command = new CreateFgsInvoiceCommand(SampleCreateDto("inv-001"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.InvoiceNumber");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateNumberExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByInvoiceNumberAsync("INV-001", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsInvoiceCommandValidator(_readRepository.Object);
        var updateDto = new FgsInvoiceUpdateDto(
            "INV-001",
            1,
            100,
            200,
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
            null,
            null,
            new DateOnly(2026, 8, 1),
            new DateOnly(2026, 8, 1),
            new DateOnly(2026, 8, 31),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            100m,
            0m,
            100m,
            8m,
            108m,
            108m);

        var result = await validator.ValidateAsync(new UpdateFgsInvoiceCommand(5, updateDto));

        result.IsValid.Should().BeTrue();
    }
}
