using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Test.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ReturnsUser()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        user.UserName.Should().Be("john_doe");
        user.Email.Should().Be("john@example.com");
        user.Password.Should().Be("HashedPass123!");
        user.UserId.Should().Be(0);
    }

    [Fact]
    public void Create_SetsIsActiveToTrue()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_SetsCreatedAtToCurrentTime()
    {
        var before = DateTime.UtcNow;
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");
        var after = DateTime.UtcNow;

        user.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public void Create_SetsUpdatedAtToNull()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        user.UpdatedAt.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyUserName_ThrowsValidationException(string userName)
    {
        var act = () => User.Create(userName, "HashedPass123!", "john@example.com");

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("UserName is required.");
    }

    [Fact]
    public void Create_WithUserNameExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => User.Create(new string('a', 51), "HashedPass123!", "john@example.com");

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("UserName must not exceed 50 characters.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyPassword_ThrowsValidationException(string password)
    {
        var act = () => User.Create("john_doe", password, "john@example.com");

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Password is required.");
    }

    [Fact]
    public void Create_WithPasswordExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => User.Create("john_doe", new string('a', 256), "john@example.com");

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Password must not exceed 255 characters.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyEmail_ThrowsValidationException(string email)
    {
        var act = () => User.Create("john_doe", "HashedPass123!", email);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Email is required.");
    }

    [Fact]
    public void Create_WithEmailExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => User.Create("john_doe", "HashedPass123!", new string('a', 101));

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Email must not exceed 100 characters.");
    }

    [Fact]
    public void Create_WithMultipleInvalidFields_ThrowsValidationExceptionWithAllErrors()
    {
        var act = () => User.Create("", "", "");

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().HaveCount(3);
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        user.Update("jane_doe", "jane@example.com", false);

        user.UserName.Should().Be("jane_doe");
        user.Email.Should().Be("jane@example.com");
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Update_WithNewPassword_UpdatesPassword()
    {
        var user = User.Create("john_doe", "OldHash", "john@example.com");

        user.Update("john_doe", "john@example.com", true, "NewHash");

        user.Password.Should().Be("NewHash");
    }

    [Fact]
    public void Update_WithNullPassword_DoesNotChangePassword()
    {
        var user = User.Create("john_doe", "OriginalHash", "john@example.com");

        user.Update("john_doe", "john@example.com", true, null);

        user.Password.Should().Be("OriginalHash");
    }

    [Fact]
    public void Update_SetsUpdatedAt()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");
        var before = DateTime.UtcNow;

        user.Update("jane_doe", "jane@example.com", true);

        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeOnOrAfter(before);
    }

    [Fact]
    public void Update_DoesNotModifyUserId()
    {
        var user = User.Reconstitute(7, "john_doe", "HashedPass123!", "john@example.com", true, DateTime.UtcNow, null);

        user.Update("jane_doe", "jane@example.com", true);

        user.UserId.Should().Be(7);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Update_WithEmptyUserName_ThrowsValidationException(string userName)
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        var act = () => user.Update(userName, "john@example.com", true);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("UserName is required.");
    }

    [Fact]
    public void Update_WithUserNameExceedingMaxLength_ThrowsValidationException()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        var act = () => user.Update(new string('a', 51), "john@example.com", true);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("UserName must not exceed 50 characters.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Update_WithEmptyEmail_ThrowsValidationException(string email)
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        var act = () => user.Update("john_doe", email, true);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Email is required.");
    }

    [Fact]
    public void Update_WithEmailExceedingMaxLength_ThrowsValidationException()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        var act = () => user.Update("john_doe", new string('a', 101), true);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Email must not exceed 100 characters.");
    }

    [Fact]
    public void Update_WithPasswordExceedingMaxLength_ThrowsValidationException()
    {
        var user = User.Create("john_doe", "HashedPass123!", "john@example.com");

        var act = () => user.Update("john_doe", "john@example.com", true, new string('a', 256));

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Password must not exceed 255 characters.");
    }
}
