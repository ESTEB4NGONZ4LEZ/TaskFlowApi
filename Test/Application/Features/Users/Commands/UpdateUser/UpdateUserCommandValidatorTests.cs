using Application.Features.Users.Commands.UpdateUser;
using Domain.Ports.Repositories;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidatorTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        _userRepositoryMock
            .Setup(r => r.ExistsWithUserNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);
        _userRepositoryMock
            .Setup(r => r.ExistsWithEmailAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new UpdateUserCommandValidator(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithValidPasswordProvided_PassesValidation()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, "Secret1@pass");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // UserId

    [Fact]
    public async Task Validate_WithNonExistentUserId_FailsWithNotFoundError()
    {
        _userRepositoryMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        var command = new UpdateUserCommand(99, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserId)
            .WithErrorMessage("User not found.");
    }

    [Fact]
    public async Task Validate_WithExistingUserId_PassesIdValidation()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.UserId);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesExistsAsyncCall()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        await _validator.TestValidateAsync(command);

        _userRepositoryMock.Verify(r => r.ExistsAsync(1), Times.Once);
    }

    // UserName

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyUserName_FailsWithRequiredError(string userName)
    {
        var command = new UpdateUserCommand(1, userName, "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserName)
            .WithErrorMessage("User Name is required.");
    }

    [Fact]
    public async Task Validate_WithUserNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateUserCommand(1, new string('a', 51), "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserName)
            .WithErrorMessage("User Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithDuplicateUserName_FailsWithUniqueError()
    {
        _userRepositoryMock
            .Setup(r => r.ExistsWithUserNameAsync("john_doe", 1))
            .ReturnsAsync(true);

        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserName)
            .WithErrorMessage("A user with this username already exists.");
    }

    [Fact]
    public async Task Validate_WithSameUserNameAsCurrentUser_PassesValidation()
    {
        _userRepositoryMock
            .Setup(r => r.ExistsWithUserNameAsync("john_doe", 1))
            .ReturnsAsync(false);

        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.UserName);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesExistsWithUserNameAsyncCall()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        await _validator.TestValidateAsync(command);

        _userRepositoryMock.Verify(r => r.ExistsWithUserNameAsync("john_doe", 1), Times.Once);
    }

    // Email

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyEmail_FailsWithRequiredError(string email)
    {
        var command = new UpdateUserCommand(1, "john_doe", email, true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public async Task Validate_WithEmailExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateUserCommand(1, "john_doe", new string('a', 101), true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email must not exceed 100 characters.");
    }

    [Fact]
    public async Task Validate_WithInvalidEmailFormat_FailsWithFormatError()
    {
        var command = new UpdateUserCommand(1, "john_doe", "not-an-email", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email must be a valid email address.");
    }

    [Fact]
    public async Task Validate_WithDuplicateEmail_FailsWithUniqueError()
    {
        _userRepositoryMock
            .Setup(r => r.ExistsWithEmailAsync("john@example.com", 1))
            .ReturnsAsync(true);

        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("A user with this email already exists.");
    }

    [Fact]
    public async Task Validate_WithSameEmailAsCurrentUser_PassesValidation()
    {
        _userRepositoryMock
            .Setup(r => r.ExistsWithEmailAsync("john@example.com", 1))
            .ReturnsAsync(false);

        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesExistsWithEmailAsyncCall()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        await _validator.TestValidateAsync(command);

        _userRepositoryMock.Verify(r => r.ExistsWithEmailAsync("john@example.com", 1), Times.Once);
    }

    // Password (optional on update)

    [Fact]
    public async Task Validate_WithNullPassword_PassesValidation()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public async Task Validate_WithPasswordTooShort_FailsWithMinLengthError()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, "Ab1@");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must be at least 8 characters.");
    }

    [Fact]
    public async Task Validate_WithPasswordExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, new string('a', 256));

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must not exceed 255 characters.");
    }

    [Theory]
    [InlineData("alllowercase1@")]
    [InlineData("ALLUPPERCASE1@")]
    [InlineData("NoDigitsHere@!")]
    [InlineData("NoSpecialChar1")]
    public async Task Validate_WithWeakPassword_FailsWithValidPasswordError(string password)
    {
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, password);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }
}
