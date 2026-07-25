using System.Net;
using System.Net.Http;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Fgs.Foundation.Constants;
using Fgs.Foundation.Middleware;
using Refit;

namespace Fgs.Foundation.Tests.Middleware;

public sealed class ExceptionMappingRulesTests
{
    [Fact]
    public void Map_KeyNotFoundException_ReturnsNotFoundWithMessage()
    {
        var (statusCode, errors) = ExceptionMappingRules.Map(new KeyNotFoundException("Entity missing."));

        statusCode.Should().Be(HttpStatusCode.NotFound);
        errors.Should().ContainSingle("Entity missing.");
    }

    [Fact]
    public void Map_InvalidOperationExceptionWithNotFound_ReturnsNotFound()
    {
        var (statusCode, errors) = ExceptionMappingRules.Map(
            new InvalidOperationException("Billing category was not found."));

        statusCode.Should().Be(HttpStatusCode.NotFound);
        errors.Should().ContainSingle("Billing category was not found.");
    }

    [Fact]
    public void Map_InvalidOperationExceptionWithoutNotFound_ReturnsConflict()
    {
        var (statusCode, errors) = ExceptionMappingRules.Map(
            new InvalidOperationException("A billing category with this combination already exists."));

        statusCode.Should().Be(HttpStatusCode.Conflict);
        errors.Should().ContainSingle("A billing category with this combination already exists.");
    }

    [Fact]
    public void Map_ArgumentException_ReturnsBadRequest()
    {
        var (statusCode, errors) = ExceptionMappingRules.Map(new ArgumentException("Tenant context is required."));

        statusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().ContainSingle("Tenant context is required.");
    }

    [Fact]
    public void Map_ValidationException_ReturnsBadRequestWithMessages()
    {
        var validation = new ValidationException(
        [
            new ValidationFailure("Code", "Code is required.")
        ]);

        var (statusCode, errors) = ExceptionMappingRules.Map(validation);

        statusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().ContainSingle("Code is required.");
    }

    [Fact]
    public void Map_UnknownException_ReturnsInternalServerErrorWithGenericMessage()
    {
        var (statusCode, errors) = ExceptionMappingRules.Map(new Exception("database exploded"));

        statusCode.Should().Be(HttpStatusCode.InternalServerError);
        errors.Should().ContainSingle(ApiErrorMessages.UnexpectedError);
    }

    [Fact]
    public async Task Map_ApiExceptionWithApiResponseBody_ReturnsDownstreamStatusAndErrors()
    {
        var apiException = await CreateApiExceptionAsync(
            HttpStatusCode.Conflict,
            """{"Success":false,"StatusCode":409,"Data":null,"Errors":["This email address is already associated with an account or pending invitation."]}""");

        var (statusCode, errors) = ExceptionMappingRules.Map(apiException);

        statusCode.Should().Be(HttpStatusCode.Conflict);
        errors.Should().ContainSingle(
            "This email address is already associated with an account or pending invitation.");
    }

    private static async Task<ApiException> CreateApiExceptionAsync(HttpStatusCode statusCode, string content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/v1/signup/company");
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };

        return await ApiException.Create(request, HttpMethod.Post, response, new RefitSettings());
    }
}
