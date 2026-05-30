using Fgs.Foundation.Result;

namespace Fgs.User.Tests.Application;

public sealed class ApiResponseTests
{
    [Fact]
    public void Ok_SetsSuccessAndData()
    {
        var response = ApiResponse<string>.Ok("value", ApiStatusCodes.Created);
        response.Success.Should().BeTrue();
        response.Data.Should().Be("value");
        response.StatusCode.Should().Be(201);
    }

    [Fact]
    public void Fail_SetsErrors()
    {
        var response = ApiResponse<string>.Fail(["error"], ApiStatusCodes.BadRequest);
        response.Success.Should().BeFalse();
        response.Errors.Should().ContainSingle("error");
    }
}
