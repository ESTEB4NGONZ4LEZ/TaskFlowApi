using Application.Features.StateTasks.Commands.UpdateStateTask;
using Domain.Ports.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.StateTasks.Commands.UpdateStateTask;

public class UpdateStateTaskCommandValidatorTests
{
    private readonly Mock<IStateTaskRepository> _stateTaskRepositoryMock;
    private readonly UpdateStateTaskCommandValidator _validator;

    public UpdateStateTaskCommandValidatorTests()
    {
        _stateTaskRepositoryMock = new Mock<IStateTaskRepository>();
        _stateTaskRepositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        _stateTaskRepositoryMock
            .Setup(r => r.ExistsWithNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new UpdateStateTaskCommandValidator(_stateTaskRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new UpdateStateTaskCommand(1, "Done", "Description");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNullDescription_PassesValidation()
    {
        var command = new UpdateStateTaskCommand(1, "Done", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNonExistentStateTaskId_FailsWithNotFoundError()
    {
        _stateTaskRepositoryMock
            .Setup(r => r.ExistsAsync(99))
            .ReturnsAsync(false);

        var command = new UpdateStateTaskCommand(99, "Done", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.StateTaskId)
            .WithErrorMessage("State task not found.");
    }

    [Fact]
    public async Task Validate_WithExistingStateTaskId_PassesIdValidation()
    {
        var command = new UpdateStateTaskCommand(1, "Done", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.StateTaskId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyName_FailsWithRequiredError(string name)
    {
        var command = new UpdateStateTaskCommand(1, name, null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public async Task Validate_WithNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateStateTaskCommand(1, new string('A', 51), null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithNameAtMaxLength_PassesValidation()
    {
        var command = new UpdateStateTaskCommand(1, new string('A', 50), null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateStateTaskCommand(1, "Done", new string('A', 151));

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 150 characters.");
    }

    [Fact]
    public async Task Validate_WithDescriptionAtMaxLength_PassesValidation()
    {
        var command = new UpdateStateTaskCommand(1, "Done", new string('A', 150));

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public async Task Validate_WithDuplicateName_FailsWithUniqueError()
    {
        _stateTaskRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Done", 1))
            .ReturnsAsync(true);

        var command = new UpdateStateTaskCommand(1, "Done", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("A state task with this name already exists.");
    }

    [Fact]
    public async Task Validate_WithSameNameAsCurrentStateTask_PassesValidation()
    {
        _stateTaskRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Done", 1))
            .ReturnsAsync(false);

        var command = new UpdateStateTaskCommand(1, "Done", null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesExistsAsyncCall()
    {
        var command = new UpdateStateTaskCommand(1, "Done", null);

        await _validator.TestValidateAsync(command);

        _stateTaskRepositoryMock.Verify(r => r.ExistsAsync(1), Times.Once);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesExistsWithNameAsyncCall()
    {
        var command = new UpdateStateTaskCommand(1, "Done", null);

        await _validator.TestValidateAsync(command);

        _stateTaskRepositoryMock.Verify(r => r.ExistsWithNameAsync("Done", 1), Times.Once);
    }
}
