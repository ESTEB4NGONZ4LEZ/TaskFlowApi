using Application.Features.Roles.Commands.UpdateRol;
using Domain.Ports.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.Roles.Commands.UpdateRol;

public class UpdateRolCommandValidatorTests
{
    private readonly Mock<IRolRepository> _rolRepositoryMock;
    private readonly UpdateRolCommandValidator _validator;

    public UpdateRolCommandValidatorTests()
    {
        _rolRepositoryMock = new Mock<IRolRepository>();
        _rolRepositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        _rolRepositoryMock
            .Setup(r => r.ExistsWithNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new UpdateRolCommandValidator(_rolRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new UpdateRolCommand(1, "Admin", "Description");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNullDescription_PassesValidation()
    {
        var command = new UpdateRolCommand(1, "Admin", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNonExistentRolId_FailsWithNotFoundError()
    {
        _rolRepositoryMock
            .Setup(r => r.ExistsAsync(99))
            .ReturnsAsync(false);

        var command = new UpdateRolCommand(99, "Admin", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.RolId)
            .WithErrorMessage("Role not found.");
    }

    [Fact]
    public async Task Validate_WithExistingRolId_PassesIdValidation()
    {
        var command = new UpdateRolCommand(1, "Admin", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.RolId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyName_FailsWithRequiredError(string name)
    {
        var command = new UpdateRolCommand(1, name, "Description");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public async Task Validate_WithNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateRolCommand(1, new string('A', 51), "Description");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithNameAtMaxLength_PassesValidation()
    {
        var command = new UpdateRolCommand(1, new string('A', 50), null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateRolCommand(1, "Admin", new string('A', 151));

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 150 characters.");
    }

    [Fact]
    public async Task Validate_WithDescriptionAtMaxLength_PassesValidation()
    {
        var command = new UpdateRolCommand(1, "Admin", new string('A', 150));

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public async Task Validate_WithDuplicateName_FailsWithUniqueError()
    {
        _rolRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Admin", 1))
            .ReturnsAsync(true);

        var command = new UpdateRolCommand(1, "Admin", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("A role with this name already exists.");
    }

    [Fact]
    public async Task Validate_WithSameNameAsCurrentRol_PassesValidation()
    {
        _rolRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Admin", 1))
            .ReturnsAsync(false);

        var command = new UpdateRolCommand(1, "Admin", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesExistsAsyncCall()
    {
        var command = new UpdateRolCommand(1, "Admin", null);

        await _validator.TestValidateAsync(command);

        _rolRepositoryMock.Verify(r => r.ExistsAsync(1), Times.Once);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesExistsWithNameAsyncCall()
    {
        var command = new UpdateRolCommand(1, "Admin", null);

        await _validator.TestValidateAsync(command);

        _rolRepositoryMock.Verify(r => r.ExistsWithNameAsync("Admin", 1), Times.Once);
    }
}
