using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Test.Domain.Entities;

public class TaskPriorityTests
{
    [Fact]
    public void Create_WithValidData_ReturnsTaskPriority()
    {
        var taskPriority = TaskPriority.Create("Critical", "Highest priority", 1);

        taskPriority.Name.Should().Be("Critical");
        taskPriority.Description.Should().Be("Highest priority");
        taskPriority.Level.Should().Be(1);
        taskPriority.TaskPriorityId.Should().Be(0);
    }

    [Fact]
    public void Create_WithNullDescription_ReturnsTaskPriority()
    {
        var taskPriority = TaskPriority.Create("Critical", null, 1);

        taskPriority.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyName_ThrowsValidationException(string name)
    {
        var act = () => TaskPriority.Create(name, null, 1);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => TaskPriority.Create(new string('A', 51), null, 1);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name must not exceed 50 characters.");
    }

    [Fact]
    public void Create_WithNameAtMaxLength_ReturnsTaskPriority()
    {
        var name = new string('A', 50);

        var taskPriority = TaskPriority.Create(name, null, 1);

        taskPriority.Name.Should().Be(name);
    }

    [Fact]
    public void Create_WithDescriptionExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => TaskPriority.Create("Critical", new string('A', 151), 1);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Description must not exceed 150 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_WithLevelLessThanOne_ThrowsValidationException(int level)
    {
        var act = () => TaskPriority.Create("Critical", null, level);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Level must be greater than or equal to 1.");
    }

    [Fact]
    public void Create_WithLevelOne_ReturnsTaskPriority()
    {
        var taskPriority = TaskPriority.Create("Critical", null, 1);

        taskPriority.Level.Should().Be(1);
    }

    [Fact]
    public void Create_WithMultipleInvalidFields_ThrowsValidationExceptionWithAllErrors()
    {
        var act = () => TaskPriority.Create("", null, 0);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var taskPriority = TaskPriority.Create("Critical", "Old description", 1);

        taskPriority.Update("High", "New description", 2);

        taskPriority.Name.Should().Be("High");
        taskPriority.Description.Should().Be("New description");
        taskPriority.Level.Should().Be(2);
    }

    [Fact]
    public void Update_WithNullDescription_SetsDescriptionToNull()
    {
        var taskPriority = TaskPriority.Create("Critical", "Some description", 1);

        taskPriority.Update("Critical", null, 1);

        taskPriority.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Update_WithEmptyName_ThrowsValidationException(string name)
    {
        var taskPriority = TaskPriority.Create("Critical", null, 1);

        var act = () => taskPriority.Update(name, null, 1);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Update_WithNameExceedingMaxLength_ThrowsValidationException()
    {
        var taskPriority = TaskPriority.Create("Critical", null, 1);

        var act = () => taskPriority.Update(new string('A', 51), null, 1);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name must not exceed 50 characters.");
    }

    [Fact]
    public void Update_WithDescriptionExceedingMaxLength_ThrowsValidationException()
    {
        var taskPriority = TaskPriority.Create("Critical", null, 1);

        var act = () => taskPriority.Update("Critical", new string('A', 151), 1);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Description must not exceed 150 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_WithLevelLessThanOne_ThrowsValidationException(int level)
    {
        var taskPriority = TaskPriority.Create("Critical", null, 1);

        var act = () => taskPriority.Update("Critical", null, level);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Level must be greater than or equal to 1.");
    }

    [Fact]
    public void Update_DoesNotModifyTaskPriorityId()
    {
        var taskPriority = TaskPriority.Reconstitute(5, "Critical", null, 1);

        taskPriority.Update("High", null, 2);

        taskPriority.TaskPriorityId.Should().Be(5);
    }
}
