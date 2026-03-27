using Application.Features.TaskPriorities.Commands.UpdateTaskPriority;
using Domain.Ports.Repositories;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.TaskPriorities.Commands.UpdateTaskPriority;

public class UpdateTaskPriorityCommandValidatorTests
{
    private readonly Mock<ITaskPriorityRepository> _taskPriorityRepositoryMock;
    private readonly UpdateTaskPriorityCommandValidator _validator;

    public UpdateTaskPriorityCommandValidatorTests()
    {
        _taskPriorityRepositoryMock = new Mock<ITaskPriorityRepository>();
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithLevelAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new UpdateTaskPriorityCommandValidator(_taskPriorityRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", "Description", 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNullDescription_PassesValidation()
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // TaskPriorityId

    [Fact]
    public async Task Validate_WithNonExistentTaskPriorityId_FailsWithNotFoundError()
    {
        _taskPriorityRepositoryMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        var command = new UpdateTaskPriorityCommand(99, "Critical", null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.TaskPriorityId)
            .WithErrorMessage("Task priority not found.");
    }

    [Fact]
    public async Task Validate_WithExistingTaskPriorityId_PassesIdValidation()
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.TaskPriorityId);
    }

    [Fact]
    public async Task Validate_VerifiesExistsAsyncCall()
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        await _validator.TestValidateAsync(command);

        _taskPriorityRepositoryMock.Verify(r => r.ExistsAsync(1), Times.Once);
    }

    // Name

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyName_FailsWithRequiredError(string name)
    {
        var command = new UpdateTaskPriorityCommand(1, name, null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public async Task Validate_WithNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateTaskPriorityCommand(1, new string('A', 51), null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithDuplicateName_FailsWithUniqueError()
    {
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Critical", 1))
            .ReturnsAsync(true);

        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("A task priority with this name already exists.");
    }

    [Fact]
    public async Task Validate_WithSameNameAsCurrentTaskPriority_PassesValidation()
    {
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Critical", 1))
            .ReturnsAsync(false);

        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_VerifiesExistsWithNameAsyncCall()
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        await _validator.TestValidateAsync(command);

        _taskPriorityRepositoryMock.Verify(r => r.ExistsWithNameAsync("Critical", 1), Times.Once);
    }

    // Description

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", new string('A', 151), 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 150 characters.");
    }

    // Level

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_WithLevelLessThanOne_FailsWithLevelError(int level)
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", null, level);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Level)
            .WithErrorMessage("Level must be greater than or equal to 1.");
    }

    [Fact]
    public async Task Validate_WithDuplicateLevel_FailsWithUniqueLevelError()
    {
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithLevelAsync(1, 1))
            .ReturnsAsync(true);

        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Level)
            .WithErrorMessage("A task priority with this level already exists.");
    }

    [Fact]
    public async Task Validate_WithSameLevelAsCurrentTaskPriority_PassesValidation()
    {
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithLevelAsync(1, 1))
            .ReturnsAsync(false);

        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Level);
    }

    [Fact]
    public async Task Validate_VerifiesExistsWithLevelAsyncCall()
    {
        var command = new UpdateTaskPriorityCommand(1, "Critical", null, 1);

        await _validator.TestValidateAsync(command);

        _taskPriorityRepositoryMock.Verify(r => r.ExistsWithLevelAsync(1, 1), Times.Once);
    }
}
