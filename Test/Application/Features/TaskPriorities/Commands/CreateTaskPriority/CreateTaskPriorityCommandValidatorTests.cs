using Application.Features.TaskPriorities.Commands.CreateTaskPriority;
using Domain.Ports.Repositories;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.TaskPriorities.Commands.CreateTaskPriority;

public class CreateTaskPriorityCommandValidatorTests
{
    private readonly Mock<ITaskPriorityRepository> _taskPriorityRepositoryMock;
    private readonly CreateTaskPriorityCommandValidator _validator;

    public CreateTaskPriorityCommandValidatorTests()
    {
        _taskPriorityRepositoryMock = new Mock<ITaskPriorityRepository>();
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithLevelAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new CreateTaskPriorityCommandValidator(_taskPriorityRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = "Highest priority", Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNullDescription_PassesValidation()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // Name

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyName_FailsWithRequiredError(string name)
    {
        var command = new CreateTaskPriorityCommand { Name = name, Description = null, Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public async Task Validate_WithNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateTaskPriorityCommand { Name = new string('A', 51), Description = null, Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithNameAtMaxLength_PassesValidation()
    {
        var command = new CreateTaskPriorityCommand { Name = new string('A', 50), Description = null, Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_WithDuplicateName_FailsWithUniqueError()
    {
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Critical", 0))
            .ReturnsAsync(true);

        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("A task priority with this name already exists.");
    }

    [Fact]
    public async Task Validate_WithDuplicateName_VerifiesRepositoryCall()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };

        await _validator.TestValidateAsync(command);

        _taskPriorityRepositoryMock.Verify(r => r.ExistsWithNameAsync("Critical", 0), Times.Once);
    }

    // Description

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = new string('A', 151), Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 150 characters.");
    }

    [Fact]
    public async Task Validate_WithDescriptionAtMaxLength_PassesValidation()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = new string('A', 150), Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    // Level

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_WithLevelLessThanOne_FailsWithLevelError(int level)
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = level };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Level)
            .WithErrorMessage("Level must be greater than or equal to 1.");
    }

    [Fact]
    public async Task Validate_WithLevelOne_PassesValidation()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Level);
    }

    [Fact]
    public async Task Validate_WithDuplicateLevel_FailsWithUniqueLevelError()
    {
        _taskPriorityRepositoryMock
            .Setup(r => r.ExistsWithLevelAsync(1, 0))
            .ReturnsAsync(true);

        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Level)
            .WithErrorMessage("A task priority with this level already exists.");
    }

    [Fact]
    public async Task Validate_WithDuplicateLevel_VerifiesRepositoryCall()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };

        await _validator.TestValidateAsync(command);

        _taskPriorityRepositoryMock.Verify(r => r.ExistsWithLevelAsync(1, 0), Times.Once);
    }
}
