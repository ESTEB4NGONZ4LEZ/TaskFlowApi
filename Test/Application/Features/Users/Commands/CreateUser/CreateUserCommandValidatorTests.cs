using Application.Features.Users.Commands.CreateUser;
using Domain.Ports.Repositories;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidatorTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepositoryMock
            .Setup(r => r.ExistsWithUserNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);
        _userRepositoryMock
            .Setup(r => r.ExistsWithEmailAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new CreateUserCommandValidator(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // UserName

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyUserName_FailsWithRequiredError(string userName)
    {
        var command = new CreateUserCommand { UserName = userName, Password = "Secret1@pass", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserName)
            .WithErrorMessage("User Name is required.");
    }

    [Fact]
    public async Task Validate_WithUserNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateUserCommand { UserName = new string('a', 51), Password = "Secret1@pass", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserName)
            .WithErrorMessage("User Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithUserNameAtMaxLength_PassesValidation()
    {
        var command = new CreateUserCommand { UserName = new string('a', 50), Password = "Secret1@pass", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.UserName);
    }

    [Fact]
    public async Task Validate_WithDuplicateUserName_FailsWithUniqueError()
    {
        _userRepositoryMock
            .Setup(r => r.ExistsWithUserNameAsync("john_doe", 0))
            .ReturnsAsync(true);

        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserName)
            .WithErrorMessage("A user with this username already exists.");
    }

    [Fact]
    public async Task Validate_WithDuplicateUserName_VerifiesRepositoryCall()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = "john@example.com" };

        await _validator.TestValidateAsync(command);

        _userRepositoryMock.Verify(r => r.ExistsWithUserNameAsync("john_doe", 0), Times.Once);
    }

    // Password

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyPassword_FailsWithRequiredError(string password)
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = password, Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public async Task Validate_WithPasswordTooShort_FailsWithMinLengthError()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Ab1@", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must be at least 8 characters.");
    }

    [Fact]
    public async Task Validate_WithPasswordExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = new string('a', 256), Email = "john@example.com" };

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
        var command = new CreateUserCommand { UserName = "john_doe", Password = password, Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }

    [Fact]
    public async Task Validate_WithValidPassword_PassesPasswordValidation()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Password);
    }

    // Email

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyEmail_FailsWithRequiredError(string email)
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = email };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public async Task Validate_WithEmailExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = new string('a', 101) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email must not exceed 100 characters.");
    }

    [Fact]
    public async Task Validate_WithInvalidEmailFormat_FailsWithFormatError()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = "not-an-email" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email must be a valid email address.");
    }

    [Fact]
    public async Task Validate_WithDuplicateEmail_FailsWithUniqueError()
    {
        _userRepositoryMock
            .Setup(r => r.ExistsWithEmailAsync("john@example.com", 0))
            .ReturnsAsync(true);

        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = "john@example.com" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("A user with this email already exists.");
    }

    [Fact]
    public async Task Validate_WithDuplicateEmail_VerifiesRepositoryCall()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@pass", Email = "john@example.com" };

        await _validator.TestValidateAsync(command);

        _userRepositoryMock.Verify(r => r.ExistsWithEmailAsync("john@example.com", 0), Times.Once);
    }
}
