using Application.Features.StateTasks.Commands.CreateStateTask;
using Domain.Ports.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.StateTasks.Commands.CreateStateTask;

public class CreateStateTaskCommandValidatorTests
{
    private readonly Mock<IStateTaskRepository> _stateTaskRepositoryMock;
    private readonly CreateStateTaskCommandValidator _validator;

    public CreateStateTaskCommandValidatorTests()
    {
        _stateTaskRepositoryMock = new Mock<IStateTaskRepository>();
        _stateTaskRepositoryMock
            .Setup(r => r.ExistsWithNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new CreateStateTaskCommandValidator(_stateTaskRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = "Description" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNullDescription_PassesValidation()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyName_FailsWithRequiredError(string name)
    {
        var command = new CreateStateTaskCommand { Name = name, Description = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public async Task Validate_WithNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateStateTaskCommand { Name = new string('A', 51), Description = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithNameAtMaxLength_PassesValidation()
    {
        var command = new CreateStateTaskCommand { Name = new string('A', 50), Description = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = new string('A', 151) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 150 characters.");
    }

    [Fact]
    public async Task Validate_WithDescriptionAtMaxLength_PassesValidation()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = new string('A', 150) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public async Task Validate_WithDuplicateName_FailsWithUniqueError()
    {
        _stateTaskRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("In Progress", 0))
            .ReturnsAsync(true);

        var command = new CreateStateTaskCommand { Name = "In Progress", Description = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("A state task with this name already exists.");
    }

    [Fact]
    public async Task Validate_WithDuplicateName_VerifiesRepositoryCall()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = null };

        await _validator.TestValidateAsync(command);

        _stateTaskRepositoryMock.Verify(r => r.ExistsWithNameAsync("In Progress", 0), Times.Once);
    }
}
