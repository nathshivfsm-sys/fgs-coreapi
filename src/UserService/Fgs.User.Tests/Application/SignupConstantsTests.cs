using Fgs.User.Application.Features.Signup;

namespace Fgs.User.Tests.Application;

public sealed class SignupConstantsTests
{
    [Fact]
    public void ToGloCreatedBy_WithProspectActor_ReturnsProspectActorUserId()
    {
        SignupConstants.ToGloCreatedBy(SignupConstants.ProspectActor)
            .Should().Be(SignupConstants.ProspectActorUserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ToGloCreatedBy_WithBlankValue_ReturnsNull(string? createdBy)
    {
        SignupConstants.ToGloCreatedBy(createdBy).Should().BeNull();
    }

    [Fact]
    public void ToGloCreatedBy_WithNumericString_ReturnsParsedUserId()
    {
        SignupConstants.ToGloCreatedBy("42").Should().Be(42);
    }

    [Fact]
    public void ToGloCreatedBy_WithUnknownActor_ReturnsNull()
    {
        SignupConstants.ToGloCreatedBy("System").Should().BeNull();
    }
}
