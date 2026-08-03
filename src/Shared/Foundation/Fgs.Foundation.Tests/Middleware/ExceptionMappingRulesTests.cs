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
    public void Map_UnauthorizedAccessException_ReturnsExactMessage()
    {
        var (statusCode, errors) = ExceptionMappingRules.Map(
            new UnauthorizedAccessException("Internal service key is missing or invalid."));

        statusCode.Should().Be(HttpStatusCode.Unauthorized);
        errors.Should().ContainSingle("Internal service key is missing or invalid.");
    }

    [Fact]
    public void Map_UnknownException_ReturnsInternalServerErrorWithExceptionMessage()
    {
        var (statusCode, errors) = ExceptionMappingRules.Map(new Exception("database exploded"));

        statusCode.Should().Be(HttpStatusCode.InternalServerError);
        errors.Should().ContainSingle("database exploded");
    }

    [Fact]
    public void Map_WrappedException_ReturnsInnermostMessage()
    {
        var inner = new InvalidOperationException("relation \"asset.FgsAsset\" does not exist");
        var outer = new Exception("An error occurred while saving the entity changes.", inner);

        var (statusCode, errors) = ExceptionMappingRules.Map(outer);

        statusCode.Should().Be(HttpStatusCode.InternalServerError);
        errors.Should().ContainSingle("relation \"asset.FgsAsset\" does not exist");
    }

    [Fact]
    public void Map_FluentValidationNullReferenceWrapper_ReturnsOuterMessage()
    {
        var inner = new NullReferenceException("Object reference not set to an instance of an object.");
        var outer = new NullReferenceException(
            "NullReferenceException occurred when executing rule for x => x.Dto.ItemCode. If this property can be null you should add a null check using a When condition",
            inner);

        var (statusCode, errors) = ExceptionMappingRules.Map(outer);

        statusCode.Should().Be(HttpStatusCode.InternalServerError);
        errors.Should().ContainSingle(outer.Message);
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
