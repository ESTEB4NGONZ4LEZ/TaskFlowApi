using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Test.Domain.Entities;

public class StateTaskTests
{
    [Fact]
    public void Create_WithValidData_ReturnsStateTask()
    {
        var stateTask = StateTask.Create("In Progress", "Task is being worked on");

        stateTask.Name.Should().Be("In Progress");
        stateTask.Description.Should().Be("Task is being worked on");
        stateTask.StateTaskId.Should().Be(0);
    }

    [Fact]
    public void Create_WithNullDescription_ReturnsStateTask()
    {
        var stateTask = StateTask.Create("Done", null);

        stateTask.Name.Should().Be("Done");
        stateTask.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyName_ThrowsValidationException(string name)
    {
        var act = () => StateTask.Create(name, null);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => StateTask.Create(new string('A', 51), null);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name must not exceed 50 characters.");
    }

    [Fact]
    public void Create_WithNameAtMaxLength_ReturnsStateTask()
    {
        var name = new string('A', 50);

        var stateTask = StateTask.Create(name, null);

        stateTask.Name.Should().Be(name);
    }

    [Fact]
    public void Create_WithDescriptionExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => StateTask.Create("Done", new string('A', 151));

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Description must not exceed 150 characters.");
    }

    [Fact]
    public void Create_WithMultipleInvalidFields_ThrowsValidationExceptionWithAllErrors()
    {
        var act = () => StateTask.Create(new string('A', 51), new string('A', 151));

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var stateTask = StateTask.Create("In Progress", "Old description");

        stateTask.Update("Done", "New description");

        stateTask.Name.Should().Be("Done");
        stateTask.Description.Should().Be("New description");
    }

    [Fact]
    public void Update_WithNullDescription_SetsDescriptionToNull()
    {
        var stateTask = StateTask.Create("In Progress", "Some description");

        stateTask.Update("In Progress", null);

        stateTask.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Update_WithEmptyName_ThrowsValidationException(string name)
    {
        var stateTask = StateTask.Create("In Progress", null);

        var act = () => stateTask.Update(name, null);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Update_WithNameExceedingMaxLength_ThrowsValidationException()
    {
        var stateTask = StateTask.Create("In Progress", null);

        var act = () => stateTask.Update(new string('A', 51), null);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name must not exceed 50 characters.");
    }

    [Fact]
    public void Update_WithDescriptionExceedingMaxLength_ThrowsValidationException()
    {
        var stateTask = StateTask.Create("In Progress", null);

        var act = () => stateTask.Update("In Progress", new string('A', 151));

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Description must not exceed 150 characters.");
    }

    [Fact]
    public void Update_DoesNotModifyStateTaskId()
    {
        var stateTask = StateTask.Reconstitute(5, "In Progress", null);

        stateTask.Update("Done", null);

        stateTask.StateTaskId.Should().Be(5);
    }
}
